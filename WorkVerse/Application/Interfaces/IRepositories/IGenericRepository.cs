using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task<T?> GetAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

        Task<PaginationResult<List<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, Func<IQueryable<T>, IQueryable<T>>? include = null,
            int? pageIndex = null, int? pageSize = null);

        void Update(T entity, bool? isOwnerRequired = false);
        void UpdateRange(List<T> entities, bool? isOwnerRequired = false);
        void SoftRemove(T entity, bool? isOwnerRequired = false);
        void SoftRemoveRange(List<T> entities, bool? isOwnerRequired = false);
        void Restore(T entity, bool? isOwnerRequired = false);
        void RestoreRange(List<T> entities, bool? isOwnerRequired = false);
        void HardRemove(T entity, bool? isOwnerRequired = false);
        void HardRemoveRange(List<T> entities, bool? isOwnerRequired = false);
    }
}
