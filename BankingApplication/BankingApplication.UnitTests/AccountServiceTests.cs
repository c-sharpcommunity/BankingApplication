using BankingApplication.Infrastructure.Dto;
using BankingApplication.Infrastructure.Entity;
using BankingApplication.Infrastructure.Repository;
using BankingApplication.Infrastructure.Service;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;

namespace BankingApplication.UnitTests
{
    public class AccountServiceTests
    {
        private IAccountService _accountService;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ITransactionRepository> _transactionRepository;

        public AccountServiceTests()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();

            _accountService = InitData();
        }

        [Fact]
        public void TestInvalidTransaction()
        {
            Transaction unknownTrans = new Transaction
            {
                Type = TransactionType.Unknown
            };
            Assert.Null(_accountService.ExecuteTransaction(null));
            Assert.Null(_accountService.ExecuteTransaction(unknownTrans));
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
            Account toAcc = _accountService.GetAccountByAccountNumber("1122");
            Transaction trans = new Transaction
            {
                ToAccount = toAcc,
                Amount = 2M,
                Type = TransactionType.Deposit,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto = _accountService.ExecuteTransaction(trans);
            Account updatedAcc = _accountService.GetAccountByAccountNumber("1122");
            Assert.NotNull(transDto);
            Assert.Equal(102M, updatedAcc.Balance);
        }

        [Fact]
        public void CheckAccountBalanceAfterTransfer()
        {
            Account fromAcc = _accountService.GetAccountByAccountNumber("0011");
            Account toAcc = _accountService.GetAccountByAccountNumber("1122");

            Transaction trans = new Transaction
            {
                FromAccount = fromAcc,
                ToAccount = toAcc,
                Amount = 2M,
                Type = TransactionType.Transfer,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto = _accountService.ExecuteTransaction(trans);

            Account updatedFromAcc = _accountService.GetAccountByAccountNumber("0011");
            Account updatedToAcc = _accountService.GetAccountByAccountNumber("1122");

            Assert.NotNull(transDto);
            Assert.Equal(98M, updatedFromAcc.Balance);
            Assert.Equal(102M, updatedToAcc.Balance);
        }

        [Fact]
        public void CheckAccountBalanceAfterWithdraw()
        {
            Account fromAcc = _accountService.GetAccountByAccountNumber("0011");
            Transaction trans = new Transaction
            {
                FromAccount = fromAcc,
                Amount = 2M,
                Type = TransactionType.Withdraw,
                CreatedDate = DateTime.Now
            };

            TransactionDto transDto = _accountService.ExecuteTransaction(trans);
            Account updatedAcc = _accountService.GetAccountByAccountNumber("0011");
            Assert.NotNull(transDto);
            Assert.Equal(98M, updatedAcc.Balance);
        }

        [Fact]
        public void CheckDuplicatedLoginNameIssue()
        {
            Account fromAcc = _accountService.GetAccountByLoginName("Tester01");
            Assert.NotNull(fromAcc);
        }


        public IAccountService InitData()
        {
            var account1 = new Account()
            {
                ID = 1,
                LoginName = "Tester01",
                Balance = 100M,
                CreatedDate = DateTime.Now,
                AccountNumber = "0011",
                Password = "64ad3fb166ddb41a2ca24f1803b8b722",
                Address = "address1"
            };

            var account2 = new Account()
            {
                ID = 2,
                LoginName = "Tester02",
                Balance = 100M,
                CreatedDate = DateTime.Now,
                AccountNumber = "1122",
                Password = "64ad3fb166ddb41a2ca24f1803b8b722",
                Address = "address2"
            };

            var account3 = new Account()
            {
                ID = 3,
                LoginName = "Tester03",
                Balance = 100M,
                CreatedDate = DateTime.Now,
                AccountNumber = "2233",
                Password = "64ad3fb166ddb41a2ca24f1803b8b722",
                Address = "address3"
            };

            var account4 = new Account()
            {
                ID = 4,
                LoginName = "Tester04",
                Balance = 100M,
                CreatedDate = DateTime.Now,
                AccountNumber = "3344",
                Password = "64ad3fb166ddb41a2ca24f1803b8b722",
                Address = "address4"
            };

            var accounts = new List<Account>();
            accounts.Add(account1);
            accounts.Add(account2);
            accounts.Add(account3);
            accounts.Add(account4);


            _accountRepository.Setup(_ => _.Delete(It.IsAny<Account>()));
            _accountRepository.Setup(_ => _.DeleteById(It.IsAny<int>()));
            _accountRepository.Setup(_ => _.GetById(It.Is<int>(x => x == 1))).Returns(account1);
            _accountRepository.Setup(_ => _.GetById(It.Is<int>(x => x == 2))).Returns(account2);
            _accountRepository.Setup(_ => _.GetByVersion(It.IsAny<int>(), It.IsAny<byte[]>())).Returns(account1);
            _accountRepository.Setup(_ => _.GetByLoginNameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(account1);
            _accountRepository.Setup(_ => _.GetByAccountNumber(It.Is<string>(x => x.Equals("0011")))).Returns(account1);
            _accountRepository.Setup(_ => _.GetByAccountNumber(It.Is<string>(x => x.Equals("1122")))).Returns(account2);
            _accountRepository.Setup(_ => _.GetByLoginName(It.IsAny<string>())).Returns(account1);
            _accountRepository.Setup(_ => _.Create(It.IsAny<Account>())).Callback<Account>(arg => accounts.Add(arg));
            _accountRepository.Setup(_ => _.Update(It.IsAny<Account>())).Callback<Account>(arg => accounts.Add(arg));
            _accountRepository.Setup(_ => _.GetAll(It.IsAny<int>())).Returns(accounts);

            return new AccountService(_accountRepository.Object, _transactionRepository.Object);
        }

    }
}
