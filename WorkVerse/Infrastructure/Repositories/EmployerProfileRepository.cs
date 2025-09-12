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
        public EmployerProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        // Kiểm tra tồn tại theo EmployerId
        public async Task<bool> ExistsByEmployerIdAsync(int employerId)
        {
            return await _dbSet.AnyAsync(j => j.EmployerId == employerId);
        }
        // Lấy tất cả employer
        public async Task<IEnumerable<EmployerProfile>> GetAllEmployersAsync()
        {
            var result = await _dbSet
                 .Include(j => j.EmployerType)
                .ToListAsync();
            return result;
        }

        // Lấy chi tiết employer theo Id
        public async Task<EmployerProfile?> GetByIdAsync(int employerId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(c => c.EmployerId == employerId);
            return result;  
        }

        // Lấy employer theo UserId
        public async Task<EmployerProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }

        // Đếm tất cả employer
        public async Task<int> CountAllEmployersAsync()
        {
            return await _dbSet.CountAsync();
        }
        // Kiểm tra tồn tại employer theo UserId
        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _dbSet.AnyAsync(j => j.UserId == userId);
        }
    }
}
