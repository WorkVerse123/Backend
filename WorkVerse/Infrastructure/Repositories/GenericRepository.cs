using Application.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly WorkVerseDBContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(WorkVerseDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<T?> GetAsync(int  id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
                query = include(query);

            // Assumes entity has a property named "Id" of type Guid
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Id");
            var idValue = Expression.Constant(id);
            var equal = Expression.Equal(property, idValue);
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }

        public void Update(T entity, bool? isOwnerRequired = false)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<T> entities, bool? isOwnerRequired = false)
        {
            _dbSet.UpdateRange(entities);
        }

        public void SoftRemove(T entity, bool? isOwnerRequired = false)
        {
            SetIsDeleted(entity, true);
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(List<T> entities, bool? isOwnerRequired = false)
        {
            foreach (var entity in entities)
            {
                SetIsDeleted(entity, true);
            }
            _dbSet.UpdateRange(entities);
        }

        public void Restore(T entity, bool? isOwnerRequired = false)
        {
            SetIsDeleted(entity, false);
            _dbSet.Update(entity);
        }

        public void RestoreRange(List<T> entities, bool? isOwnerRequired = false)
        {
            foreach (var entity in entities)
            {
                SetIsDeleted(entity, false);
            }
            _dbSet.UpdateRange(entities);
        }

        public void HardRemove(T entity, bool? isOwnerRequired = false)
        {
            _dbSet.Remove(entity);
        }

        public void HardRemoveRange(List<T> entities, bool? isOwnerRequired = false)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Sets the IsDeleted property if it exists.
        /// </summary>
        private void SetIsDeleted(T entity, bool value)
        {
            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                prop.SetValue(entity, value);
            }
        }
    }
}
