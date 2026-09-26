using BankAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Services
{
    public class TransferService
    {
        private readonly BankDbContext context;

        [HttpPost("transfer")]
        public async Task<string> Transfer()
        {

            return "good";
        }
    }
}
