using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionManager.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<List<T>> Find(Func<T, bool> predicate);
        Task Create(T item);
        Task CreateMany(List<T> items);
        Task Update(T item);
        Task Delete(int id);
        Task Save();
    }
}
