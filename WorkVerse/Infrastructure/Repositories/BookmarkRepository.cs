using Application.DTOs.Response;
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
    public class BookmarkRepository : GenericRepository<Bookmark>, IBookmarkRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public BookmarkRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Bookmark>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbContext.Bookmarks
                .Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.SavedAt)
                .ToListAsync();

            return result;
        }

        public async Task<Bookmark?> ExistsAsync(int employeeId, int jobId)
        {
            var result = await _dbContext.Bookmarks.Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(b => b.EmployeeId == employeeId && b.JobId == jobId);
            return result;
        }

        public async Task<Bookmark> GetByIdAsync(int bookmarkId)
        {
            var result = await _dbContext.Bookmarks.FirstOrDefaultAsync(u => u.BookmarkId == bookmarkId);
            return result;
        }

    }
}
