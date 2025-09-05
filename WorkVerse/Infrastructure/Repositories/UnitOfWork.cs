using Application.Interfaces.IReposervices;
using Application.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WorkVerseDBContext _dbContext;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(WorkVerseDBContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            EmployeeProfile = new EmployeeProfileRepository(_dbContext);
            BusyTime = new BusyTimeRepository(_dbContext);
            Bookmark = new BookmarkRepository(_dbContext);
            Job = new JobRepository(_dbContext);
            Application = new ApplicationRepository(_dbContext);
            JobCategory = new JobCategoryRepository(_dbContext);
            Review = new ReviewRepository(_dbContext);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                _logger.LogWarning("A transaction is already in progress.");
                return;
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
            _logger.LogInformation("Database transaction started.");
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                _logger.LogWarning("No transaction to commit.");
                return;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                _logger.LogInformation("Database transaction committed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error committing transaction.");
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                _logger.LogWarning("No transaction to rollback.");
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync();
                _logger.LogInformation("Database transaction rolled back.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back transaction.");
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task<ITransaction?> GetCurrentTransactionAsync()
        {
            return _currentTransaction != null ? new EfCoreTransaction(_currentTransaction) : null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        // Helper class to adapt EF Core transaction to your ITransaction interface
        private class EfCoreTransaction : ITransaction
        {
            private readonly IDbContextTransaction _transaction;

            public EfCoreTransaction(IDbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public async ValueTask DisposeAsync()
            {
                await _transaction.DisposeAsync();
            }

            public async Task CommitAsync()
            {
                await _transaction.CommitAsync();
            }

            public async Task RollbackAsync()
            {
                await _transaction.RollbackAsync();
            }
        }

        //This place to start progress dependency injection
        public IEmployeeProfileRepository EmployeeProfile { get; private set; }
        public IBusyTimeRepository BusyTime { get; private set; }
        public IBookmarkRepository Bookmark { get; private set; }
        public IJobRepository Job { get; private set; }
        public IApplicationRepository Application { get; private set; }

        public IJobCategoryRepository JobCategory { get; private set; }

        public IReviewRepository Review { get; private set; }
    }
}
