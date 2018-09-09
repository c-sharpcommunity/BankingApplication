using BankingApplication.Infrastructure.Entity;

namespace BankingApplication.Infrastructure.Repository
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        void SaveTransaction(Transaction entity);
    }
}
