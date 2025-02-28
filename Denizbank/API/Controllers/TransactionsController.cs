using System.Net.Http.Headers;
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

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
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
                    Description = request.Description
                };
            }
            else
            {
                return BadRequest("Birşeyler ters gitti");
            }
        }
    }
}
