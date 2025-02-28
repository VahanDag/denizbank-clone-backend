using System.ComponentModel;

namespace Denizbank.Core.Enums
{
    public enum CardType
    {
        Credit,
        Debit
    }

    public enum SystemCards
    {
        [Description("DenizBank Banka")]
        DenizBankDebit,

        [Description("DenizBank Black")]
        DenizBankCreditBlack,

        [Description("DenizBank Gold")]
        DenizBankCreditGold,

        [Description("DenizBank Platinum")]
        DenizBankCreditPlatinum
    }
}