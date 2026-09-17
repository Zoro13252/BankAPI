//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using BankAPI.Models;
//using BankAPI.Data;
//using Microsoft.EntityFrameworkCore;
//using BankAPI.DTOs;

//namespace BankAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountsController : ControllerBase
//    {
//        private readonly BankDbContext context;

//        public AccountsController(BankDbContext context)
//        {
//            this.context = context;
//        }

//        [HttpGet("{userId}/accounts")]
//        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(int userId)
//        { 
//            var result = await this.context.Accounts
//                .Where(a => a.UserId == userId)
//                .Select(a => new Account
//                {
//                    Id = a.Id,
//                    UserId = a.UserId,
//                    AccountNumber = a.AccountNumber,
//                    Balance = a.Balance,
//                    Currency = a.Currency,
//                    CreatedAt = a.CreatedAt
//                })
//                .ToListAsync();

            
//            Console.WriteLine($"Найдено счетов для пользователя {userId}: {result.Count}");
//            foreach (var account in result)
//            {
//                Console.WriteLine($"-> №{account.AccountNumber} | Баланс: {account.Balance} {account.Currency}");
//            }

//            return Ok(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreatAccount([FromBody] CreatAccountsDto dto)
//        {
//            var newAccount = new Account
//            {
//                AccountNumber = dto.AccountNumber,
//                Balance = dto.Balance,
//            };
//           await this.context.AddAsync(newAccount);
//            return Ok();
//        }

//    }
//}
