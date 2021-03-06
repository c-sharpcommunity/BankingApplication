﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Infrastructure.Entity
{
    public class Account
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string AccountNumber { get; set; }        
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
