using System.Security.Claims;
using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.DTOs;
using Denizbank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Denizbank.API.Controllers
{
    public class CardsController: ControllerBase
    {
        private readonly CardService _cardService;
        private readonly AccountService _accountService;

        public CardsController(CardService cardService, AccountService accountService)
        {
            _cardService = cardService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("myCards")]
        public async Task<ActionResult<IEnumerable<CardDto>>> GetMyCards(CardRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var account = await _accountService.GetAccountById(uint.Parse(userId));

            if (account == null) return NotFound();

            var cards = await _cardService.GetCards(account, request);

            var cardsDtos = cards.Select(c => new CardDto
            {
                Id = c.Id,
                AccountId = c.AccountId,
                CardNumber = c.CardNumber,
                CSV = c.CSV,
                ExpirationDate = c.ExpirationDate,
                Balance = c.Balance,
                CardType = c.CardType,
                BalanceLimit = c.BalanceLimit,
                Debt = c.Debt,
                CutOfDate = c.CutOfDate,
                IBAN = c.IBAN
            });

            return Ok(cardsDtos);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("createDenizBankCard")]
        public async Task<ActionResult<DenizBankCard>> CreateDenizBankCard(DenizbankCardCreateRequest request)
        {
            var response = await _cardService.CreateCard(request);
            return response == null ? BadRequest() : response;
        }

        [Authorize]
        [HttpGet("getDenizBankCards")]
        public async Task<ActionResult<IEnumerable<DenizBankCard>>> GetDenizBankCards(CardType? cardType)
        {
            var response = await _cardService.GetAllDenizBankCards(cardType);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("createCardForUser")]
        public async Task<ActionResult<CardDto>> CreateCardForUser(CreateCardForUserRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var account = await _accountService.GetAccountById(uint.Parse(userId));

            if (account == null) return NotFound();

            var response = await _cardService.CreateCardForUser(uint.Parse(userId), request);

            return response.IsSuccess ? Ok(response.Data) : BadRequest(response.ErrorMessage);
        }

    }
}
