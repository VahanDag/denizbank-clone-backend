namespace Denizbank.Core.Models
{
    public class Account
    {
        public uint Id { get; set; }

        public required string Name { get; set; }

        public required ulong TcNo { get; set; } // 11 digits

        public required int Password { get; set;}
        public string Roles { get; set; } = "User";


        //public decimal Balance { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
           
    }
}
