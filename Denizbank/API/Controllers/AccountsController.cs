using System.Security.Claims;
using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.Data;
using Denizbank.DTOs;
using Denizbank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Denizbank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController: ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly AccountService _accountService;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountsController(ILogger<AccountsController> logger, IConfiguration configuration, TokenService tokenService ,AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request) 
        {
            var account = await _accountService.Login(request.TcNo, request.Password);

            if (account == null) return Unauthorized("Yanlış TC No veya Şifre");

            var token = _tokenService.CreateToken(account);

            return new LoginResponse
            {
                AccountId = account.Id,
                Name = account.Name,
                Token = token
            };
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<AccountDto>> GetMyAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var account = await _accountService.GetAccountById(uint.Parse(userId));

            if (account == null) return NotFound();

            var accountDto = new AccountDto
            {
                Id = account.Id,
                Name = account.Name,
                TcNo = account.TcNo,
                Cards = account.Cards?.Select(c => new CardDto
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

                }).ToList() ?? new List<CardDto>()
            };

            return accountDto;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            var account = new Account { Name = request.Name, Password = request.Password, TcNo = request.TcNo };
            var response = await _accountService.CreateAccount(account);

            if (response == null)
            {
                return BadRequest(new { error = "Bu TC numarası ile kayıtlı bir hesap zaten mevcut." });
            }

            return new RegisterResponse
            {
                Name = response.Name,
                AccountId = response.Id
            };
        }

        [HttpPost("register-admin")]
        public async Task<ActionResult<RegisterResponse>> RegisterAdmin(RegisterRequest request, [FromHeader(Name = "Admin-Secret")] string adminSecret)
        {
            // read secret admin key from appsettings.json
            if (adminSecret != _configuration["AdminSecret"])
            {
                return Unauthorized("Admin hesabı oluşturmak için geçerli bir yetkilendirme gerekiyor");
            }

            var acount = new Account { Name = request.Name, Password = request.Password, TcNo = request.TcNo };
            var response = await _accountService.CreateAccount(acount, "Admin");

            if (response == null)
            {
                return BadRequest(new { error = "Bu TC numarası ile kayıtlı bir hesap zaten mevcut." });

            }
            return new RegisterResponse
            {
                Name = response.Name,
                AccountId = response.Id
            };

        }

    }
}
