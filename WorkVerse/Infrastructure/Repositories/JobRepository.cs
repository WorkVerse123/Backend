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
        private readonly WorkVerseDBContext _dbContext;
        public JobRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Job> GetByIdAsync(int jobId)
        {
            var result = await _dbContext.Jobs
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .FirstOrDefaultAsync(u => u.JobId == jobId);
            return result;
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            var result = await _dbContext.Jobs
                .Include(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }

        public async Task<bool> ExistsAsync(int jobId)
        {
            return await _dbContext.Jobs.AnyAsync(j => j.JobId == jobId);
        }

    }
}
