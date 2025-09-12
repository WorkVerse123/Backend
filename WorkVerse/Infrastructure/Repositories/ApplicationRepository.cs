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
        public ApplicationRepository(WorkVerseDBContext context) : base(context)
        {
        }

        // lấy danh sách ứng tuyển theo employeeId
        public async Task<IEnumerable<Domain.Entities.Application>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbSet
                 .Include(b => b.Job)
                 .ThenInclude(j => j.JobCategoryMappings)
                 .ThenInclude(m => m.Category)
                 .Where(b => b.EmployeeId == employeeId)
                 .OrderByDescending(b => b.AppliedAt)
                 .ToListAsync();

            return result;
        }

        // lấy chi tiết 1 application theo Id
        public async Task<Domain.Entities.Application> GetByIdAsync(int applicationId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(u => u.ApplicationId == applicationId);
            return result;
        }

        // Tìm application theo CandidateId + JobId (nếu đã apply)
        public async Task<Domain.Entities.Application?> FindByEmployeeAndJobAsync(int employeeId, int jobId)
        {
            var result = await _dbSet.Include(b => b.Job)
                .ThenInclude(j => j.JobCategoryMappings)
                .ThenInclude(m => m.Category)
                .Where(b => b.EmployeeId == employeeId && b.JobId == jobId)
                .OrderByDescending(b => b.AppliedAt)
                .FirstOrDefaultAsync();
            return result;
        }

        // Kiểm tra application có tồn tại không theo ApplicationId
        public async Task<bool> ExistsByIdAsync(int applicationId)
        {
            return await _dbSet
                .AnyAsync(r => r.ApplicationId == applicationId);
        }

        // Lấy danh sách application theo JobId (nhà tuyển dụng xem ứng viên ứng tuyển)
        public async Task<IEnumerable<Domain.Entities.Application>> GetByJobIdAsync(int jobId)
        {
            var result = await _dbSet
                .Include(b => b.Employee)
                .Where(b => b.JobId == jobId)
                .OrderByDescending(b => b.AppliedAt)
                .ToListAsync();

            return result;
        }
    }
}
