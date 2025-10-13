using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Blog>> GetAllBlogListsAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Blog?> GetBlogDetailsBySlugAsync(string slug)
        {
            return await _dbSet.Include(c => c.Author.StaffProfile).FirstOrDefaultAsync(b => b.Slug == slug);
        }
        public async Task<Blog?> GetBlogDetailsByIdAsync(int blogId)
        {
            return await _dbSet.Include(c => c.Author.StaffProfile).FirstOrDefaultAsync(b => b.BlogId == blogId );
        }
        public async Task<Blog?> GetBlogByIdAsync(int blogId)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.BlogId == blogId);
        }

        public async Task<bool> CheckBlogsExistBySlugAsync(string slug, int? excludeId = null)
        {
            var query = _dbSet.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(b => b.BlogId != excludeId.Value);
            }

            return await query.AnyAsync(b => b.Slug == slug);
        }

    }
}
