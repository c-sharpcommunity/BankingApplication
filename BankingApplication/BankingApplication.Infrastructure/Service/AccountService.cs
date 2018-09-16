using BankingApplication.Infrastructure.Dto;
using BankingApplication.Infrastructure.Entity;
using BankingApplication.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BankingApplication.Infrastructure.Service
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRespository;
        private ITransactionRepository _transactionRespository;

        public AccountService(IAccountRepository accountRespository, ITransactionRepository transactionRespository)
        {
            _accountRespository = accountRespository;
            _transactionRespository = transactionRespository;

        }
        public AccountDto CreateAccount(Account user)
        {
            AccountDto accountDto = new AccountDto();
            accountDto.Account = user;
            try
            {
                _accountRespository.Create(user);
            }
            catch (Exception ex)
            {
                accountDto.Errors.Add("CreateAccount", ex.Message);

                return accountDto;
            }

            return accountDto;
        }
        
        public Account GetAccount(string loginName, string password)
        {
            return _accountRespository.GetByLoginNameAndPassword(loginName, password);
        }
        public Account GetAccountByLoginName(string loginName)
        {
            return _accountRespository.GetByLoginName(loginName);
        }
        public Account GetAccountByAccountNumber(string accountNumber)
        {
            return _accountRespository.GetByAccountNumber(accountNumber);
        }
        public Account GetAccount(int id)
        {
            return _accountRespository.GetById(id);
        }

        public IEnumerable<Account> GetListAllUser()
        {
            return _accountRespository.GetAll(0);
        }

        public AccountDto UpdateAccount(Account user)
        {
            var accountDto = new AccountDto();

            var foundAccount = _accountRespository.GetByVersion(user.ID, user.RowVersion);


            if (foundAccount == null)
            {
                return CreateAccount(user);
            }
            else
            {
                foundAccount.LoginName = user.LoginName;

                foundAccount.RowVersion = user.RowVersion;

                foundAccount.Address = user.Address;

                try
                {
                    _accountRespository.Update(foundAccount);
                    accountDto.Account = foundAccount;
                    return accountDto;
                }
                catch (DBConcurrencyException ex)
                {
                    accountDto.Account = user;
                    return accountDto;
                }
            }
        }

        public TransactionDto ExecuteTransaction(Transaction trans)
        {
            if (trans == null) return null;

            var transDto = new TransactionDto();

            Account toAccount = trans.ToAccount == null ? null : _accountRespository.GetById(trans.ToAccount.ID);
            Account fromAccount = trans.FromAccount == null ? null : _accountRespository.GetById(trans.FromAccount.ID);
            
            try
            {
                string connectionString = "Server = localhost; Database = BankingApplication; Trusted_Connection = True; ";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                switch (trans.Type)
                {
                    case TransactionType.Deposit:
                        string validateMsg = ValidateDepositTransaction(trans);
                        if (!string.IsNullOrEmpty(validateMsg))
                        {
                            transDto.Errors.Add("DepositTransaction", validateMsg);
                            return transDto;
                        }
                        toAccount.Balance += trans.Amount;

                        break;
                    case TransactionType.Withdraw:
                        validateMsg = ValidateWithdrawTransaction(trans);
                        if (!string.IsNullOrEmpty(validateMsg))
                        {
                            transDto.Errors.Add("WithdrawTransaction", validateMsg);
                            return transDto;
                        }
                        fromAccount.Balance -= trans.Amount;

                        break;
                    case TransactionType.Transfer:
                        validateMsg = ValidateTransferTransaction(trans);
                        if (!string.IsNullOrEmpty(validateMsg))
                        {
                            transDto.Errors.Add("TransferTransaction", validateMsg);
                            return transDto;
                        }
                        fromAccount.Balance -= trans.Amount;
                        toAccount.Balance += trans.Amount;

                        break;
                    default: return null;
                }

                trans.FromAccount = fromAccount;
                trans.ToAccount = toAccount;
                trans.Status = TransactionStatus.Success;

                if (toAccount != null) toAccount.RowVersion = trans.ToAccount.RowVersion;
                if (fromAccount != null) fromAccount.RowVersion = trans.FromAccount.RowVersion;
                
                _transactionRespository.SaveTransaction(trans);
            }

            catch (DBConcurrencyException ex)
            {
                throw ex;
            }
            transDto.Transaction = trans;
            return transDto;

        }

        public IEnumerable<Transaction> GetTheirOwnTransactions(int id)
        {
            return _transactionRespository.GetAll(id);
        }

        #region Static Methods

        public static string ValidateDepositTransaction(Transaction trans)
        {
            if (trans == null || trans.Amount <= 0) return "Invalid amount";
            return string.Empty;
        }
        public static string ValidateTransferTransaction(Transaction trans)
        {
            var errMsg = string.Empty;

            if (trans == null || trans.Amount <= 0 || trans.FromAccount.Balance < trans.Amount) errMsg = "Invalid amount";
            if (trans.FromAccount == null || string.IsNullOrEmpty(trans.FromAccount.AccountNumber)) errMsg += " Invalid FromAccount";
            if (trans.ToAccount == null || string.IsNullOrEmpty(trans.ToAccount.AccountNumber)) errMsg += " Invalid ToAccount";

            return string.Empty;
        }
        public static string ValidateWithdrawTransaction(Transaction trans)
        {
            if (trans == null || trans.Amount <= 0 || trans.FromAccount.Balance < trans.Amount) return "Invalid amount";
            return string.Empty;
        }

        #endregion
    }
}
