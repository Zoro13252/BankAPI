using BankAPI.Data;
using BankAPI.Models;
using BankAPI.DTOs.TransferDTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class TransferService
    {
        private readonly BankDbContext context;

        public TransferService(BankDbContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResult> ExecuteTransferAsync(int fromUserId, TransferDto dto)
        {
            using var transaction = await this.context.Database.BeginTransactionAsync();
            try
            {
                var userFrom = await this.context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.Id == fromUserId);

                if (userFrom == null)
                    return ServiceResult.Fail("Отправитель не найден");

                if (userFrom.Accounts == null || !userFrom.Accounts.Any())
                    return ServiceResult.Fail("У вас нет активных счетов");

                
                var userTo = await this.context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.PhoneNumber == dto.phoneNumber);

                if (userTo == null)
                    return ServiceResult.Fail($"Пользователь с номером {dto.phoneNumber} не найден");

                if (userTo.Accounts == null || !userTo.Accounts.Any())
                    return ServiceResult.Fail("У пользователя нет активных счетов");

                var accBalanceUserFrom = userFrom.Accounts.First();
                var accBalanceUserTo = userTo.Accounts.First();

                
                if (accBalanceUserFrom.Balance < dto.amount)
                {
                    return ServiceResult.Fail("Недостаточно средств на счете");
                }

                
                accBalanceUserFrom.Balance -= dto.amount;
                accBalanceUserTo.Balance += dto.amount;

                await this.context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ServiceResult.Ok();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
