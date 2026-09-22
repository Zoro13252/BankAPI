using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankAPI.Data;
using BankAPI.DTOs;
using BankAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankAPI.Services;

public class UserService
{
    private readonly BankDbContext context;
    private readonly IConfiguration config;

    public UserService(BankDbContext context, IConfiguration config)
    {
        this.context = context;
        this.config = config;
    }

    public async Task<CreateUserDto?> Get(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return null;

        return new CreateUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };
    }

    /// <summary>
    /// Регистрация нового пользователя + возврат JWT
    /// </summary>
    public async Task<AuthResponseDto> Register(CreateUserDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email уже занят");

        var newUser = new User
        {
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            Email = dto.Email,
            PhoneNumber = dto.Phone ?? string.Empty,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            Accounts = new List<Account>
            {
                new Account
                {
                    AccountNumber = dto.Account.AccountNumber,
                    Balance = dto.Account.Balance,
                    Currency = dto.Account.Currency
                }
            }
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return GenerateToken(newUser);
    }

    /// <summary>
    /// Вход по email + password, возвращает JWT
    /// </summary>
    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Неверный email или пароль");

        return GenerateToken(user);
    }

    public async Task<List<GetUsersDto>> GetAll()
    {
        return await context.Users
            .Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<User> ChangeData(ChangeUserDto dto)
    {
        var user = await context.Users.FindAsync(dto.userId);
        if (user == null)
            throw new Exception("Пользователь с таким ID не найден.");

        if (!string.IsNullOrEmpty(dto.firstName)) user.FirstName = dto.firstName;
        if (!string.IsNullOrEmpty(dto.lastName)) user.LastName = dto.lastName;
        if (!string.IsNullOrEmpty(dto.email)) user.Email = dto.email;

        await context.SaveChangesAsync();
        return user;
    }

    public async Task<string> DeletUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) throw new Exception("NotFound");

        context.Remove(user);
        await context.SaveChangesAsync();
        return $"User by ID {id} deleted";
    }

    private AuthResponseDto GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expireMinutes = int.Parse(config["Jwt:ExpireMinutes"] ?? "60");

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
