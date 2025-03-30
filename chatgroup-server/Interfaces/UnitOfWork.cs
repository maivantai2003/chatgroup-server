
using chatgroup_server.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace chatgroup_server.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransactionAsync()
        {
            _transaction=await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction has not been started.");
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
