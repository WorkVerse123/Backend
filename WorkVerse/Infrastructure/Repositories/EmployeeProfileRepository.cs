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
    public class EmployeeProfileRepository : GenericRepository<EmployeeProfile>, IEmployeeProfileRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public EmployeeProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    

        public async Task<EmployeeProfile?> GetByIdAsync(int employeeId)
        {
            var result = await _dbContext.EmployeeProfiles.Include(c => c.User).FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            return result;
        }
        public async Task<EmployeeProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbContext.EmployeeProfiles.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }

        public async Task<bool> ExistsAsync(int employeeId)
        {
            return await _dbContext.EmployeeProfiles.AnyAsync(j => j.EmployeeId == employeeId);
        }

        public async Task<IEnumerable<EmployeeProfile>> GetAllCandidatesAsync()
        {
            var result = await _dbContext.EmployeeProfiles.Where(u => u.Mode == "public").ToListAsync();
            return result;
        }
    }
}
