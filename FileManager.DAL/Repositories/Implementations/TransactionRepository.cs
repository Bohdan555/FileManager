using TransactionManager.DAL.EF;
using TransactionManager.DAL.Entities;
using TransactionManager.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionManager.DAL.Repositories.Implementations
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly TransactionManagerContext _context;

        public TransactionRepository(TransactionManagerContext context) 
        {
            _context = context;
        }

        public async Task Create(Transaction item)
        {
            await _context.Transactions.AddAsync(item);
        }

        public async Task CreateMany(List<Transaction> items)
        {
            await _context.AddRangeAsync(items);
        }

        public async Task Delete(int id)
        {
            var transaction = await _context.Transactions
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
            if (transaction != null)
                _context.Remove(transaction);
        }

        public async Task<List<Transaction>> Find(Func<Transaction, bool> predicate)
        {
            var transactions = _context.Transactions
                                .Include(t => t.Status)
                                .Where(predicate: predicate)                                
                                .ToList();

            return await Task.FromResult(transactions);
        }

        public async Task<Transaction> Get(int id)
        {
            return await _context.Transactions
                .Where(t => t.Id == id)
                .Include(t => t.Status)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetAll()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task Update(Transaction item)
        {
            var entity = _context.Transactions.FindAsync(item.Id);
            if (entity == null)
            {
                return;
            }

            _context.Entry(entity).CurrentValues.SetValues(item);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}