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
        private readonly WorkVerseDBContext _dbContext;
        public ReviewRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Review> GetByIdAsync(int reviewId)
        {
            var result = await _dbContext.Reviews           
                 .FirstOrDefaultAsync(c => c.ReviewId == reviewId);
            return result;
        }

        public async Task<IEnumerable<Review>> GetByJobIdAsync(int jobId)
        {
            var result = await _dbContext.Reviews.Where(c => c.JobId == jobId)
                .OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }

        public async Task<Review> GetByJobIdEmployeeIdAsync(int jobId, int employeeId)
        {
            var result = await _dbContext.Reviews
                 .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.JobId == jobId);
            return result;
        }

        public async Task<bool> ExistsAsync(int jobId, int employeeId)
        {
            return await _dbContext.Reviews
                .AnyAsync(r => r.JobId == jobId && r.EmployeeId == employeeId);
        }

    }
}
