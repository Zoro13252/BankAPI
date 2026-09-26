using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Models;
using BankAPI.Data;
using Microsoft.EntityFrameworkCore;
using BankAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly BankDbContext context;

        

    }
}
