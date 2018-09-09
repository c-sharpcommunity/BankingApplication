using System.Collections.Generic;

namespace BankingApplication.Infrastructure.Repository
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll(int id);
        T GetById(int id);
        T GetByEmail(string email);
        T GetByNumber(string number);
        T GetByEmailAndPassword(string email, string password);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(int id);
    }
}
