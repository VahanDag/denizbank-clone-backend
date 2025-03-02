using System.Net.Http.Headers;
using System.Security.Claims;
using Denizbank.Core.Enums;
using Denizbank.Core.Models;
using Denizbank.DTOs;
using Denizbank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Denizbank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController: ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly AccountService _accountService;

        public TransactionsController(TransactionService transactionService, AccountService accountService)
        {
            _transactionService = transactionService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpPost("transferMoney")]
        public async Task<ActionResult<TransferResponse>> transferMoney(TransferRequest request)
        {
            if(!request.FromIBAN.StartsWith("TR") || !request.ToIBAN.StartsWith("TR"))
            {
                ModelState.AddModelError("IBAN", "IBAN 'TR' ile başlamalıdır");
                return BadRequest(ModelState);
            };
            
            if(request.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Tutar 0'dan büyük olmalıdır");
                return BadRequest(ModelState);
            }

            var transferResponse = await _transactionService.TransferMoney(request);

            if (transferResponse != null)
            {
                return new TransferResponse
                {
                    Amount = transferResponse.Amount,
                    FromIBAN = request.FromIBAN,
                    ToIBAN = request.ToIBAN,
                    FromName = transferResponse.FromAccount?.Name ?? "Bilinmeyen",
                    ToName = transferResponse.ToAccount?.Name ?? "Bilinmeyen",
                    Date = transferResponse.Date,
                    Description = request.Description,
                    Status = transferResponse.Status
                };
            }
            else
            {
                return BadRequest("Birşeyler ters gitti");
            }
        }

        [Authorize]
        [HttpGet("getTransactions")]
        public async Task<ActionResult<IEnumerable<TransferResponse>>> GetTransactions([FromQuery] TransactionType? transactionType = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("Yetkisiz Erişim");
            var account = await _accountService.GetAccountById(uint.Parse(userId));
            if (account == null) return NotFound("Kullanıcı Bulunamadı");

            var response = await _transactionService.GetTransactions(transactionType);

            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest("İşlem verileir çekilirken hata oluştu!");
            }
        }
    }
}
