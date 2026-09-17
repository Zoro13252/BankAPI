namespace BankAPI.Models;

    public class Account
    {
   
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "RUB";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User? User { get; set; } = null!;
    }

