using Denizbank.Core.Enums;

namespace Denizbank.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        // Transaction Date
        public DateTime Date { get; set; }

        // Transaction type (deposit, withdrawal, transfer)
        public TransactionType Type { get; set; }

        // Transaction amount
        public decimal Amount { get; set; }

        public uint FromAccountId { get; set; }
        public Account FromAccount { get; set; } = null!;

        // Recipient account (for transfer transactions)
        public uint? ToAccountId { get; set; }
        public Account? ToAccount { get; set; }

        // Transaction Description
        public string? Description { get; set; }

        // Transaction Status
        public TransactionStatus Status { get; set; }
    }



}
