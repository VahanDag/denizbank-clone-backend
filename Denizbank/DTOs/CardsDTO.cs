using System.ComponentModel.DataAnnotations;
using Denizbank.Core.Enums;
using Denizbank.Core.Models;

namespace Denizbank.DTOs
{

    public class CardRequest
    {
        public CardType? CardType { get; set; }
    }

    public class CardDto
    {
        public int Id { get; set; }
        public uint AccountId { get; set; }
        public string CardNumber { get; set; }
        public DenizBankCard CardType { get; set; }
        public ushort CSV { get; set; }
        public string ExpirationDate { get; set; }
        public decimal Balance { get; set; }
        public decimal? BalanceLimit { get; set; }
        public decimal? Debt { get; set; }
        public ushort? CutOfDate { get; set; }
        public string? IBAN { get; set; }
    }

    public class DenizbankCardCreateRequest
    {
        [Required(ErrorMessage ="Kart tipi boş olamaz (Debit/Credit)")]
        public CardType CardType { get; set; }
        
        [Required(ErrorMessage = "Kart adı boş olamaz")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Kart açıklaması boş olamaz")]
        public string CardDescription { get; set; }

        [Required(ErrorMessage ="Kart resmi boş olamaz")]
        public string ImageURI { get; set; }
    }   

    public class CreateCardForUserRequest
    {
        [Required(ErrorMessage ="Bir kart seçilmeli")]
        public int DenizBankCardId { get; set; }
        public CardType CardType { get; set; }

    }

}
