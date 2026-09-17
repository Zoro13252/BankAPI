namespace BankAPI.DTOs
{
    public class ChangeUserDto
    {
        public ChangeUserDto(int userId, string? firstName, string? lastName, string? email, string? phone)
        {
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phone = phone;
        }
        public int userId { get; set; }
        public string? firstName { get; set; } = null;
        public string? lastName { get; set; } = null;
        public string? email { get; set; } = null;
        public string? phone { get; set; } = null;
    }
}
