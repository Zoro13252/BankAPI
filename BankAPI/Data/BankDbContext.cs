using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data
{
    public class BankDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public BankDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this.configuration.GetConnectionString("DataBase"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.UserFrom)
                .WithMany(u => u.SentTransactions)
                .HasForeignKey(t => t.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.UserTo)
                .WithMany(u => u.ReceivedTransactions)
                .HasForeignKey(t => t.UserToId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
