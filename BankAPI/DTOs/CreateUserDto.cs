namespace BankAPI.DTOs
{
    public class CreateUserDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = null;
        public string Password { get; set; } = string.Empty;

        public CreatAccountsDto Account { get; set; } = new();
    }
}
