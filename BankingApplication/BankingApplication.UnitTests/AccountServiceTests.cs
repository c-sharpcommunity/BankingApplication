using BankingApplication.Infrastructure.Dto;
using BankingApplication.Infrastructure.Entity;
using BankingApplication.Infrastructure.Repository;
using BankingApplication.Infrastructure.Service;
using System;
using Xunit;

namespace BankingApplication.UnitTests
{
    public class AccountServiceTests
    {
        private IAccountRepository _accountRespository;
        private ITransactionRepository _transactionRespository;

        [Fact]
        public void TestInvalidTransaction()
        {
            Transaction unknownTrans = new Transaction
            {
                Type = TransactionType.Unknown
            };
            Assert.Null((new AccountService(_accountRespository, _transactionRespository)).ExecuteTransaction(null));
            Assert.Null((new AccountService(_accountRespository, _transactionRespository)).ExecuteTransaction(unknownTrans));
        }

        [Fact]
        public void CheckValidWithdrawAmount_Balance()
        {
            Transaction trans = new Transaction
            {
                FromAccount = new Account { Balance = 9 },
                Amount = 10
            };
            string msg = AccountService.ValidateWithdrawTransaction(trans);

            Assert.Equal("Invalid amount", msg);

        }

        [Fact]
        public void CheckValidWithdrawAmount_PositiveAmount()
        {
            Transaction trans = new Transaction
            {
                FromAccount = new Account { Balance = 10 },
                Amount = 10
            };
            string msg = AccountService.ValidateWithdrawTransaction(trans);

            Assert.Equal("", msg);

        }

        [Fact]
        public void CheckValidWithdrawAmount_NegativeAmount()
        {
            Transaction trans = new Transaction
            {
                Amount = -10
            };
            string msg = AccountService.ValidateWithdrawTransaction(trans);

            Assert.Equal("Invalid amount", msg);

        }

        [Fact]
        public void CheckValidWithdrawAmount_Null()
        {
            string msg = AccountService.ValidateWithdrawTransaction(null);

            Assert.Equal("Invalid amount", msg);
        }

        [Fact]
        public void CheckValidDepositAmount_PositiveAmount()
        {
            Transaction trans = new Transaction
            {
                Amount = 10
            };
            string msg = AccountService.ValidateDepositTransaction(trans);

            Assert.Equal("", msg);
        }

        [Fact]
        public void CheckValidDepositAmount_NegativeAmount()
        {
            Transaction trans = new Transaction
            {
                Amount = -10
            };
            string msg = AccountService.ValidateDepositTransaction(trans);

            Assert.Equal("Invalid amount", msg);
        }

        [Fact]
        public void CheckValidDepositAmount_Null()
        {
            string msg = AccountService.ValidateDepositTransaction(null);

            Assert.Equal("Invalid amount", msg);
        }

        [Fact]
        public void CheckAccountBalanceAfterDeposit()
        {
            Account toAcc = _accountRespository.GetByAccountNumber("1122");
            Transaction trans = new Transaction
            {
                ToAccount = toAcc,
                Amount = 2M,
                Type = TransactionType.Deposit,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto =  (new AccountService(_accountRespository, _transactionRespository)).ExecuteTransaction(trans);
            Account updatedAcc = _accountRespository.GetByAccountNumber("1122");
            Assert.NotNull(transDto);
            Assert.Equal(102M, updatedAcc.Balance);
        }

        [Fact]
        public void CheckAccountBalanceAfterTransfer()
        {
            Account toAcc = _accountRespository.GetByAccountNumber("0101");
            Account fromAcc = _accountRespository.GetByAccountNumber("1212");

            Transaction trans = new Transaction
            {
                FromAccount = fromAcc,
                ToAccount = toAcc,
                Amount = 2M,
                Type = TransactionType.Transfer,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto = (new AccountService(_accountRespository, _transactionRespository)).ExecuteTransaction(trans);
            Account updatedToAcc = _accountRespository.GetByAccountNumber("0101");
            Account updatedFromAcc = _accountRespository.GetByAccountNumber("1212");

            Assert.NotNull(transDto);
            Assert.Equal(98M, updatedFromAcc.Balance);
            Assert.Equal(102M, updatedToAcc.Balance);
        }

        [Fact]
        public void CheckAccountBalanceAfterWithdraw()
        {
            Account fromAcc = _accountRespository.GetByAccountNumber("3434");
            Transaction trans = new Transaction
            {
                FromAccount = fromAcc,
                Amount = 2M,
                Type = TransactionType.Withdraw,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto = (new AccountService(_accountRespository, _transactionRespository)).ExecuteTransaction(trans);
            Account updatedAcc = _accountRespository.GetByAccountNumber("3434");
            Assert.NotNull(transDto);
            Assert.Equal(98M, updatedAcc.Balance);
        }

    }
}
