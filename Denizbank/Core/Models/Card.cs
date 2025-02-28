using Denizbank.Core.Enums;

namespace Denizbank.Core.Models
{
    public class Card
    {
        public int Id { get; set; }

        // Foreign key
        public required uint AccountId { get; set; }

        // Navigation property
        public  required Account Account { get; set; }

        public required string CardNumber { get; set; }

        public required ushort CSV { get; set; }

        public required string ExpirationDate { get; set; } // ex: 09/25

        public required decimal Balance { get; set; }

        public DenizBankCard CardType { get; set; }
        public decimal? BalanceLimit { get; set; } // if type is Credit

        public decimal? Debt { get; set; } // Borç

        public ushort? CutOfDate { get; set; } // ex: 17: Her ayın 17'si

        public string? IBAN { get; set; }

    }

    /// <summary>
    /// All Available Card Created By System(Denizbank)
    /// For example: Denizbank Black, Denizbank Banka Kartı, Denizbank Bonus etc.
    /// </summary>
    public class DenizBankCard
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public CardType CardType { get; set; } // Debit, Credit
        public string CardDescription { get; set; }
        public string ImageURI { get; set; } = "https://sea.mastercard.com/content/dam/public/mastercardcom/sea/en/smb/cards/professional-credit-card_1280x720.png";

    }
}
