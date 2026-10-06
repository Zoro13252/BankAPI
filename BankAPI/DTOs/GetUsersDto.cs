namespace BankAPI.DTOs
{
    public class GetUsersDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? Balance { get; set; }
        public string? AccountNumber { get; set; }

    }
}
