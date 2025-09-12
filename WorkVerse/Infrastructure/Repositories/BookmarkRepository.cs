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
        public BookmarkRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        // Lấy danh sách bookmark của 1 employee
        public async Task<IEnumerable<Bookmark>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbSet
                .Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.SavedAt)
                .ToListAsync();

            return result;
        }

        // Tìm bookmark theo CandidateId + JobId (nếu đã lưu)
        public async Task<Bookmark?> FindByEmployeeAndJobAsync(int employeeId, int jobId)
        {
            var result = await _dbSet.Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(b => b.EmployeeId == employeeId && b.JobId == jobId);
            return result;
        }

        // Lấy chi tiết 1 bookmark theo Id
        public async Task<Bookmark> GetByIdAsync(int bookmarkId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(u => u.BookmarkId == bookmarkId);
            return result;
        }

    }
}
