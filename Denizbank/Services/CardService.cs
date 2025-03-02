using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.Core.Results;
using Denizbank.Core.Utilities;
using Denizbank.Data;
using Denizbank.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Denizbank.Services
{
    public class CardService
    {
        private readonly BankingDbContext _bankingDbContext;

        public CardService(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }

        public async Task<IEnumerable<Card>> GetCards(Account account, CardRequest request)
        {
            var query = _bankingDbContext.Cards
                .Where(c => c.AccountId == account.Id);
            if (request.CardType != null)
            {
                query = query.Where(c => c.CardType.CardType == request.CardType);
            }
            var cards = await query.ToListAsync();
            return cards;
        }

        public async Task<IEnumerable<DenizBankCard>> GetAllDenizBankCards(CardType? cardType)
        {
            IQueryable<DenizBankCard> query = _bankingDbContext.DenizBankCard.AsNoTracking();

            if (cardType != null)
            {
                query = query.Where(c => c.CardType == cardType);
            }

            return await query.ToListAsync();
        }

        public async Task<DenizBankCard> CreateCard(DenizbankCardCreateRequest request)
        {
            var denizbankCard = new DenizBankCard
            {
                CardDescription = request.CardDescription,
                CardName = request.CardName,
                CardType = request.CardType
            };

            _bankingDbContext.DenizBankCard.Add(denizbankCard);
            await _bankingDbContext.SaveChangesAsync();

            return denizbankCard;
        }

        public async Task<ServiceResult<CardDto>> CreateCardForUser(uint userId, CreateCardForUserRequest request)
        {
            var user = await _bankingDbContext.Accounts
                .Include(a => a.Cards)
                    .ThenInclude(c => c.CardType)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user == null)
                return ServiceResult<CardDto>.Failure("Böyle bir kullanıcı yok!");

            var cardsWithRequestedType = user.Cards.Count(c => c.CardType.CardType == request.CardType);
            if (cardsWithRequestedType >= 3)
            {
                return ServiceResult<CardDto>.Failure("En fazla 3 karta sahip olabilirsiniz!");
            }

            var selectedCard = await _bankingDbContext.DenizBankCard.FirstOrDefaultAsync(c => c.Id == request.DenizBankCardId);
            if (selectedCard == null)
                return ServiceResult<CardDto>.Failure("Bu kart kullanım dışı!");

            var newCardRecord = new Card
            {
                Account = user,
                AccountId = user.Id,
                Balance = 0,
                CardType = selectedCard,
                CardNumber = CardUtilities.GenerateCardNumber(),
                CSV = CardUtilities.GenerateCsv(),
                ExpirationDate = CardUtilities.GenerateExpirationDate(),
                BalanceLimit = request.CardType == CardType.Credit ? 42000 : null,
                CutOfDate = request.CardType == CardType.Credit ? CardUtilities.GenerateCutOfDate() : null,
                Debt = request.CardType == CardType.Credit ? 0 : null,
                IBAN = request.CardType == CardType.Credit ? null : CardUtilities.GenerateIBAN()
            };

            _bankingDbContext.Cards.Add(newCardRecord);
            await _bankingDbContext.SaveChangesAsync();

            var cardDto = new CardDto
            {
                Id = newCardRecord.Id,
                AccountId = newCardRecord.AccountId,
                CardNumber = newCardRecord.CardNumber,
                CSV = newCardRecord.CSV,
                ExpirationDate = newCardRecord.ExpirationDate,
                Balance = newCardRecord.Balance,
                BalanceLimit = newCardRecord.BalanceLimit,
                CardType = newCardRecord.CardType,
                CutOfDate = newCardRecord.CutOfDate,
                Debt = newCardRecord.Debt,
                IBAN = newCardRecord.IBAN,
            };


            return ServiceResult<CardDto>.Success(cardDto);
        }
    }

}