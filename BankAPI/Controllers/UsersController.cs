using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankAPI.DTOs;
using BankAPI.Models;
using BankAPI.Services;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // все эндпоинты требуют JWT
public class UsersController : ControllerBase
{
    private readonly UserService userService;

    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetAll()
    {
        var data = await userService.GetAll();
        return Ok(data);
    }

    [HttpPatch("update")]
    public async Task<ActionResult<User>> ChangeData([FromBody] ChangeUserDto dto)
    {
        try
        {
            var data = await userService.ChangeData(dto);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await userService.DeletUserAsync(id);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreateUserDto>> Get(int id)
    {
        var user = await userService.Get(id);
        if (user == null)
            return NotFound(new { message = "Пользователь не найден" });

        return Ok(user);
    }
}
