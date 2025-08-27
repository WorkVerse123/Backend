using Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<List<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, Func<IQueryable<T>, IQueryable<T>>? include = null, int? pageIndex = null, int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            throw new NotImplementedException();
        }

        public void HardRemove(T entity, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void HardRemoveRange(List<T> entities, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void Restore(T entity, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void RestoreRange(List<T> entities, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void SoftRemove(T entity, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void SoftRemoveRange(List<T> entities, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(List<T> entities, bool? isOwnerRequired = false)
        {
            throw new NotImplementedException();
        }
    }
}
