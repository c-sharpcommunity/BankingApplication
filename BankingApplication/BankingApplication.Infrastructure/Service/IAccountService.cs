using BankingApplication.Infrastructure.Dto;
using BankingApplication.Infrastructure.Entity;
using System.Collections.Generic;

namespace BankingApplication.Infrastructure.Service
{
    public interface IAccountService
    {
        Account GetAccount(string email, string password);
        Account GetAccountByLoginName(string loginName);
        Account GetAccountByAccountNumber(string accountNumber);
        Account GetAccount(int id);
        IEnumerable<Account> GetListAllUser();
        AccountDto CreateAccount(Account user);
        AccountDto UpdateAccount(Account user);
        TransactionDto ExecuteTransaction(Transaction trans);
        IEnumerable<Transaction> GetTheirOwnTransactions(int id); 
    }
}
