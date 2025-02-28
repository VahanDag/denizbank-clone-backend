namespace Denizbank.Core.Enums
{
    public enum TransactionType
    {
        Deposit,     // Para Yatırma
        Withdrawal,  // Para Çekme
        Transfer     // Havale/EFT
    }

    public enum TransactionStatus
    {
        Pending,   // Beklemede
        Completed, // Tamamlandı
        Failed     // Başarısız
    }
}
