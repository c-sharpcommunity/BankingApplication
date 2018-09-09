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
                //var result = await _dbContext.SaveChangesAsync();
                _accountRespository.Create(user);
            }
            catch (Exception ex)
            {

                accountDto.Errors.Add("CreateAccount", ex.Message);

                return accountDto;
            }

            return accountDto;
        }
        
        public Account GetAccount(string email, string password)
        {
            return _accountRespository.GetByEmailAndPassword(email, password);
        }
        public Account GetAccountByEmail(string email)
        {
            return _accountRespository.GetByEmail(email);
        }
        public Account GetAccountByNumber(string number)
        {
            return _accountRespository.GetByNumber(number);
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
                foundAccount.Address = user.Address;
                foundAccount.FullName = user.FullName;

                foundAccount.RowVersion = user.RowVersion;

                try
                {
                    //var result = await _dbContext.SaveChangesAsync();
                    _accountRespository.Update(foundAccount);
                    accountDto.Account = foundAccount;
                    return accountDto;
                }
                catch (DBConcurrencyException ex)
                {
                    accountDto.Account = user;
                    GenerateAccountErrorsLog(string.Empty, foundAccount, ex, accountDto.Errors);
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
                string connectionString = "Server = localhost; Database = SimpleBankApp; Trusted_Connection = True; ";
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
                //wrong account info or balance
                if (trans.ToAccount != null) GenerateAccountErrorsLog("ToAccount", trans.ToAccount, ex, transDto.Errors);
                if (trans.FromAccount != null) GenerateAccountErrorsLog("FromAccount", trans.FromAccount, ex, transDto.Errors);
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
            if (trans.FromAccount == null || string.IsNullOrEmpty(trans.FromAccount.Number)) errMsg += " Invalid FromAccount";
            if (trans.ToAccount == null || string.IsNullOrEmpty(trans.ToAccount.Number)) errMsg += " Invalid ToAccount";

            return string.Empty;
        }
        public static string ValidateWithdrawTransaction(Transaction trans)
        {
            if (trans == null || trans.Amount <= 0 || trans.FromAccount.Balance < trans.Amount) return "Invalid amount";
            return string.Empty;
        }


        private Dictionary<string, string> GenerateAccountErrorsLog(string prefix, Account accountToUpdate, DBConcurrencyException ex, Dictionary<string, string> errors)
        {
            string strErrMsg = ex.Message;
            string strRowErrMsg = ex.Row[0].ToString();

            if (strRowErrMsg == null)
            {
                errors.Add("DeletedAccount", "Could not save changes. The account was deleted.");
            }
            else
            {
                //var databaseValues = (Account)databaseEntry.ToObject();
                //if (databaseValues.FullName != clientValues.FullName)
                //{
                //    if (string.IsNullOrEmpty(prefix)) errors.Add("FullName", $"Current value: {databaseValues.FullName}");
                //    else errors.Add($"{prefix}.FullName", $"Current value: {databaseValues.FullName}");
                //}
                //if (databaseValues.Address != clientValues.Address)
                //{
                //    if (string.IsNullOrEmpty(prefix)) errors.Add("Address", $"Current value: {databaseValues.Address}");
                //    else errors.Add($"{prefix}.Address", $"Current value: {databaseValues.Address}");
                //}
                //if (databaseValues.Balance != clientValues.Balance)
                //{
                //    if (string.IsNullOrEmpty(prefix)) errors.Add("Address", $"Current value: {databaseValues.Address}");
                //    else errors.Add($"{prefix}.Balance", $"Current value: {databaseValues.Balance}");
                //}

                //accountToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                accountToUpdate.RowVersion = new byte[2];
            }

            return errors;
        }

        #endregion
    }
}
