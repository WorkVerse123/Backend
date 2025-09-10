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
    public class EmployerProfileRepository : GenericRepository<EmployerProfile>, IEmployerProfileRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public EmployerProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(int employerId)
        {
            return await _dbContext.EmployerProfiles.AnyAsync(j => j.EmployerId == employerId);
        }

        public async Task<IEnumerable<EmployerProfile>> GetAllCompaniesAsync()
        {
            var result = await _dbContext.EmployerProfiles
                 .Include(j => j.EmployerType)
                .ToListAsync();
            return result;
        }

        public async Task<EmployerProfile?> GetByIdAsync(int employerId)
        {
            var result = await _dbContext.EmployerProfiles.FirstOrDefaultAsync(c => c.EmployerId == employerId);
            return result;
        }

        public async Task<EmployerProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbContext.EmployerProfiles.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }

        public async Task<int> CountCompaniesAsync()
        {
            return await _dbContext.EmployerProfiles.CountAsync();
        }

        public async Task<bool> CheckExistByUserIdAsync(int userId)
        {
            return await _dbContext.EmployerProfiles.AnyAsync(j => j.UserId == userId);
        }
    }
}
