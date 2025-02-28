using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Denizbank.DTOs
{
    public class TransferRequest
    {
        [Required(ErrorMessage = "IBAN zorunludur")]
        [RegularExpression(@"^TR\d+$", ErrorMessage = "IBAN 'TR' ile başlamalıdır")]
        public string FromIBAN { get; set; }

        [Required(ErrorMessage = "IBAN zorunludur")]
        [RegularExpression(@"^TR\d+$", ErrorMessage = "IBAN 'TR' ile başlamalıdır")]
        public string ToIBAN { get; set; }

        [Required(ErrorMessage = "Para miktarı girilmesi zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Amount { get; set; }

        public string? Description { get; set; }
    }

    public class TransferResponse
    {
        public string FromIBAN { get; set; }
        public string ToIBAN { get; set; }
        public DateTime Date { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }

    }
}