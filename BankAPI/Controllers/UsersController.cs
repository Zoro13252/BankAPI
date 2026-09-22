using Microsoft.AspNetCore.Mvc;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using BankAPI.Services;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;

        public UsersController(BankDbContext context, UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            return Ok(await this.userService.CreateUser(dto));
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var data = await this.userService.GetAll();
            return Ok(data);
        }

        [HttpPatch("update")]
        public async Task<ActionResult<User>> ChangeData(ChangeUserDto dto)
        {
            var data = await this.userService.ChangeData(dto);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletUserAsync(int id)
        {
            var data = await this.userService.DeletUserAsync(id);
            return Ok(data);
        }

        [HttpGet("get{id}")]
        public async Task<ActionResult<CreateUserDto>> Get(int id)
        {
            var user = await this.userService.Get(id);
            return user;
        }



    }
}