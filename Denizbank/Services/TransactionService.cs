using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.Data;
using Denizbank.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Denizbank.Services
{
    public class TransactionService
    {

        private readonly BankingDbContext _bankingDbContext;

        public TransactionService(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }

        public async Task<Transaction?> TransferMoney(TransferRequest request)
        {
            using var transaction = await _bankingDbContext.Database.BeginTransactionAsync();

            try
            {
                var fromAccountCard = await _bankingDbContext.Cards
                    .Include(c => c.Account) 
                    .FirstOrDefaultAsync(a => a.IBAN == request.FromIBAN);

                if (fromAccountCard == null || fromAccountCard.CardType.CardType != CardType.Debit || fromAccountCard.IBAN == null || fromAccountCard.Balance < request.Amount)
                {
                    return null;
                }

                var toAccountCard = await _bankingDbContext.Cards
                    .Include(c => c.Account) 
                    .FirstOrDefaultAsync(a => a.IBAN == request.ToIBAN);

                if (toAccountCard == null || toAccountCard.CardType.CardType != CardType.Debit || toAccountCard.IBAN == null)
                {
                    return null;
                }

                fromAccountCard.Balance -= request.Amount;
                toAccountCard.Balance += request.Amount;

                var transactionRecord = new Transaction
                {
                    FromAccount = fromAccountCard.Account,
                    FromAccountId = fromAccountCard.AccountId,
                    ToAccount = toAccountCard.Account,
                    ToAccountId = toAccountCard.AccountId,
                    Amount = request.Amount,
                    Date = DateTime.UtcNow,
                    Description = request.Description ?? "",
                    Type = TransactionType.Transfer,
                    Status = TransactionStatus.Completed,
                };

                _bankingDbContext.Transactions.Add(transactionRecord);
                await _bankingDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return transactionRecord;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

    }
}
