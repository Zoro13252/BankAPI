using BankAPI.Models;

public class Transaction
{
    public int Id { get; set; }
    public int Value { get; set; }

    public int UserFromId { get; set; }
    public User UserFrom { get; set; }

    public int UserToId { get; set; }
    public User UserTo { get; set; }

    public DateTime ExecutedIn { get; set; } = DateTime.UtcNow;
}