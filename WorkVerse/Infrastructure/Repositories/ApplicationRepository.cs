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
    public class ApplicationRepository : GenericRepository<Domain.Entities.Application>, IApplicationRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public ApplicationRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Entities.Application>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbContext.Applications
                 .Include(b => b.Job)
                 .ThenInclude(j => j.JobCategoryMappings)
                 .ThenInclude(m => m.Category)
                 .Where(b => b.EmployeeId == employeeId)
                 .ToListAsync();

            return result;
        }

        public async Task<Domain.Entities.Application> GetByIdAsync(int applicationId)
        {
            var result = await _dbContext.Applications.FirstOrDefaultAsync(u => u.ApplicationId == applicationId);
            return result;
        }

        public async Task<Domain.Entities.Application?> ExistsAsync(int employeeId, int jobId)
        {
            var result = await _dbContext.Applications.Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .Where(b => b.EmployeeId == employeeId && b.JobId == jobId)
                .OrderByDescending(b => b.AppliedAt)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}
