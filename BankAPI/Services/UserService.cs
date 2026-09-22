using BCrypt.Net;
using BankAPI.Data;
using BankAPI.DTOs;
using BankAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class UserService
    {
        private readonly BankDbContext context;
        public UserService(BankDbContext context)
        {
            this.context = context;
        }

        public async Task<CreateUserDto> Get(int id)
        {
            var user = await this.context.Users.FindAsync(id);
            var response = new CreateUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber
                
            };

            return (response);
        }

        public async Task<ActionResult<CreateUserDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            var newUser = new User
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
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

            

            var response = new CreateUserDto
            {
                LastName = newUser.LastName,
                FirstName = newUser.FirstName,
                Email = newUser.Email
                

            };

            await this.context.Users.AddAsync(newUser);
            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Настоящая причина здесь:
                var innerException = ex.InnerException;

                // Для отладки можно вывести на консоль:
                Console.WriteLine(innerException?.Message);
            }

            return (response);
        }

        public async Task<ActionResult<List<GetUsersDto>>> GetAll()
        {
            var result = await this.context.Users.Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            }).ToListAsync();
            return (result);
        }

        public async Task<ActionResult<User>> ChangeData(ChangeUserDto dto)
        {
            var user = await this.context.Users.FindAsync(dto.userId);
            if (user == null)
            {
                throw new Exception("Пользователь с таким ID не найден.");
            }

            if (!string.IsNullOrEmpty(dto.firstName)) user.FirstName = dto.firstName;
            if (!string.IsNullOrEmpty(dto.lastName)) user.LastName = dto.lastName;
            if (!string.IsNullOrEmpty(dto.email)) user.Email = dto.email;
            await this.context.SaveChangesAsync();
            return (user);
        }

        public async Task<ActionResult<string>> DeletUserAsync(int id)
        {
            var user = await this.context.Users.FindAsync(id);
            if (user == null) throw new Exception("NotFound");
            this.context.Remove(user);
            await this.context.SaveChangesAsync();
            return $"User by ID {id} deleted";
        }


    }
}
