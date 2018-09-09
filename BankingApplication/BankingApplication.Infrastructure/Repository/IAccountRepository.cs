using BankingApplication.Infrastructure.Entity;

namespace BankingApplication.Infrastructure.Repository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Account GetByVersion(int id, byte[] rowVersion);
    }
}
