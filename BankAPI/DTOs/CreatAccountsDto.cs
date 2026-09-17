using BankAPI.Models;

namespace BankAPI.DTOs
{
    public class CreatAccountsDto
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "RUB";
    }
}
