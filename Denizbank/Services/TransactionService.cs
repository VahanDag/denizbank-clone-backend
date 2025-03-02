using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.Core.Results;
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
                  .Include(c => c.CardType) 
                  .FirstOrDefaultAsync(a => a.IBAN == request.FromIBAN);

                if (fromAccountCard == null ||
                    fromAccountCard.CardType == null ||
                    fromAccountCard.CardType.CardType != CardType.Debit ||
                    fromAccountCard.IBAN == null ||
                    fromAccountCard.Balance < request.Amount)
                {
                    return null;
                }

                var toAccountCard = await _bankingDbContext.Cards
                    .Include(c => c.Account)
                    .Include(c => c.CardType)
                    .FirstOrDefaultAsync(a => a.IBAN == request.ToIBAN);

                if (toAccountCard == null ||
                    toAccountCard.CardType == null || 
                    toAccountCard.CardType.CardType != CardType.Debit ||
                    toAccountCard.IBAN == null)
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
        public async Task<ServiceResult<IEnumerable<TransferResponse>>> GetTransactions(TransactionType? transactionType = null)
        {
            IQueryable<Transaction> query = _bankingDbContext.Transactions
                .Include(t => t.FromAccount)
                    .ThenInclude(a => a.Cards)
                .Include(t => t.ToAccount)
                    .ThenInclude(a => a.Cards)
                .AsNoTracking();

            if (transactionType != null)
            {
                query = query.Where(t => t.Type == transactionType);
            }

            var transactions = await query.ToListAsync();

            var transactionResponses = transactions.Select(t => new TransferResponse
            {
                Amount = t.Amount,
                Date = t.Date,
                Description = t.Description,
                Status = t.Status,
                FromIBAN = t.FromAccount?.Cards.FirstOrDefault(c => c.IBAN != null)?.IBAN ?? "Bilinmiyor",
                ToIBAN = t.ToAccount?.Cards.FirstOrDefault(c => c.IBAN != null)?.IBAN ?? "Bilinmiyor",
                FromName = t.FromAccount?.Name ?? "Bilinmiyor",
                ToName = t.ToAccount?.Name ?? "Bilinmiyor",
                TransactionType = t.Type
            }).ToList();

            return ServiceResult<IEnumerable<TransferResponse>>.Success(transactionResponses);
        }

    }
}
