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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }

        // Lấy 1 review theo Id
        public async Task<Review> GetByIdAsync(int reviewId)
        {
            var result = await _dbSet           
                 .FirstOrDefaultAsync(c => c.ReviewId == reviewId);
            return result;
        }

        // Lấy tất cả review theo JobId
        public async Task<IEnumerable<Review>> GetByJobIdAsync(int jobId)
        {
            var result = await _dbSet.Where(c => c.JobId == jobId)
                .OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }
        // Lấy review của 1 employee cho 1 job
        public async Task<Review> GetByJobAndCandidateAsync(int jobId, int employeeId)
        {
            var result = await _dbSet
                 .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.JobId == jobId);
            return result;
        }

        // Kiểm tra review tồn tại theo job và candidate
        public async Task<bool> ExistsByJobAndCandidateAsync(int jobId, int employeeId)
        {
            return await _dbSet
                .AnyAsync(r => r.JobId == jobId && r.EmployeeId == employeeId);
        }

    }
}
