namespace BankAPI.Models;

    public class User
    {
        public User()
        {

        }
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Transaction> SentTransactions { get; set; }
        public ICollection<Transaction> ReceivedTransactions { get; set; }

        public List<Account> Accounts { get; set; } = new();

        void ChangeData()
        {
            
        }
    }

