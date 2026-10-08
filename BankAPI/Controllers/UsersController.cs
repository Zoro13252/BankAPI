using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankAPI.DTOs;
using BankAPI.Models;
using BankAPI.Services;
using BankAPI.DTOs.TransferDTOs;
using System.Security.Claims;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly UserService userService;
    private readonly TransferService transferService;

    public UsersController(UserService userService, TransferService transferService)
    {
        this.userService = userService;
        this.transferService = transferService;
    }

    [Authorize(Roles = "PremiumUser")]
    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetAll()
    {
        var data = await userService.GetAll();
        return Ok(data);
    }
    [Authorize]
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

    [Authorize(Roles = "PremiumUser")]
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

    [Authorize]
    [HttpPost("transfer")]
    public async Task<ActionResult> Transfers([FromBody] TransferDto Tdto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int fromUserId))
        {
            return Unauthorized("Не удалось определить пользователя по токену");
        }


        await transferService.ExecuteTransferAsync(fromUserId, Tdto);
        return Ok("Перевод успешно выполнен");
    }
}
