using Denizbank.Core.Enums;
using Denizbank.Core.Extensions;
using Denizbank.Core.Models;
using Denizbank.Core.Utilities;
using Denizbank.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Denizbank.Services
{
    public class AccountService
    {
        private readonly BankingDbContext _dbContext;

        public AccountService(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account?> Login(ulong tcNo, int password)
        {
            try
            {
                var account = await _dbContext.Accounts
                    .Include(a => a.Cards)
                    .FirstOrDefaultAsync(predicate: a => a.TcNo == tcNo && a.Password == password);

                Console.WriteLine($"Login attempt: TcNo={tcNo}, Found={account != null}");

                return account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                throw; 
            }
        }

        public async Task<Account?> GetAccountById(uint id)
        {
            return await _dbContext.Accounts.Include(a => a.Cards).ThenInclude(c=> c.CardType).Include(a => a.Transactions).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> CreateAccount(Account account, string role = "User")
        {
            // TC Number validation
            var existingAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.TcNo == account.TcNo);

            if (existingAccount != null)
            {
                return null; // for now
            }

            account.Roles = role;

            _dbContext.Accounts.Add(account);

            // get available debit card
            var debitCard = await _dbContext.DenizBankCard.FirstOrDefaultAsync(c => c.CardType == CardType.Debit);

            if (debitCard == null)
            {
                throw new InvalidOperationException("Herhangi bir banka kartı bulunamadı!");
            }

            // create First debit card automatically for user
            var card = new Card
            {
                Account = account,
                AccountId = account.Id,
                Balance = 10000,
                CardType = debitCard,
                CardNumber = CardUtilities.GenerateCardNumber(),
                CSV = CardUtilities.GenerateCsv(),
                ExpirationDate = CardUtilities.GenerateExpirationDate(),
                IBAN = CardUtilities.GenerateIBAN()

            };

            _dbContext.Cards.Add(card);
            
            await _dbContext.SaveChangesAsync();

            return account;
        }
    } 
}
