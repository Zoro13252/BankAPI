using BankAPI.Models;
namespace BankAPI.DTOs.TransferDTOs
{
    public class TransferDto
    {
        public string phoneNumber { get; set; }
        public decimal? amount { get; set; }
    }
}
