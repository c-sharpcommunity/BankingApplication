using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Infrastructure.Entity
{
    public class Account
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Address { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
