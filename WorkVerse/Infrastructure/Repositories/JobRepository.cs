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
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        // Lấy 1 job theo Id
        public async Task<Job> GetByIdAsync(int jobId)
        {
            var result = await _dbSet
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(u => u.JobId == jobId);
            return result;
        }
        // Lấy tất cả job
        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            var result = await _dbSet
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .OrderByDescending(u => u.IsPriority)
                .ThenByDescending(c => c.CreatedAt).ToListAsync();

            return result;
        }
        // Kiểm tra tồn tại job theo Id
        public async Task<bool> ExistsByJobIdAsync(int jobId)
        {
            return await _dbSet.AnyAsync(j => j.JobId == jobId);
        }

        // Đếm tất cả job
        public async Task<int> CountAllJobsAsync()
        {
            return await _dbSet.CountAsync();
        }

        // Đếm job mới trong range time
        public async Task<int> CountNewJobsAsync(TimeSpan range)
        {
            var fromDate = DateTime.Now.Subtract(range);
            return await _dbSet
                .Where(j => j.CreatedAt >= fromDate)
                .CountAsync();
        }

        // Lấy danh sách tất cả job theo EmployerId
        public async Task<IEnumerable<Job>> GetJobsByEmployerIdAsync(int employerId)
        {
            var result = await _dbSet
                .Where(j => j.EmployerId == employerId)
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }
        // Lay title cua job bang jobId
        public async Task<string?> GetJobTitleByIdAsync(int id)
        {
            return await _dbSet
                .Where(j => j.JobId == id)
                .Select(j => j.Title)
                .FirstOrDefaultAsync();
        }

    }
}
