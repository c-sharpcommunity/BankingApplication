using System;

namespace BankingApplication.Infrastructure.Entity
{
    public class Transaction
    {
        public int ID { get; set; }
        public Account ToAccount { get; set; }
        public int ToAccountId { get; set; }
        public Account FromAccount { get; set; }
        public int FromAccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

    }

    public enum TransactionType
    {
        Unknown,
        Deposit,
        Withdraw,
        Transfer
    }
    public enum TransactionStatus
    {
        Processing = 1,
        Success = 2
    }
}
