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
        public EmployeeProfileRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }

        // Lấy profile theo EmployeeId
        public async Task<EmployeeProfile?> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            return result;
        }


        // Lấy profile theo UserId
        public async Task<EmployeeProfile?> GetByUserIdAsync(int userId)
        {
            var result = await _dbSet.Include(c => c.User).FirstOrDefaultAsync(u => u.UserId == userId);
            return result;
        }
        // Kiểm tra profile tồn tại  theo employeeId
        public async Task<bool> ExistsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.AnyAsync(j => j.EmployeeId == employeeId);
        }

        // Lấy tất cả Employee public
        public async Task<IEnumerable<EmployeeProfile>> GetAllPublicEmployeeAsync()
        {
            var result = await _dbSet.Where(u => u.Mode == "public").ToListAsync();
            return result;
        }
        // Đếm tất cả employee
        public async Task<int> CountAllEmployeeAsync()
        {
            return await _dbSet.CountAsync();
        }

    }
}
