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
    public class BusyTimeRepository : GenericRepository<BusyTime>, IBusyTimeRepository
    {
        public BusyTimeRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }

        // Lấy tất cả BusyTime của 1 employee
        public async Task<IEnumerable<BusyTime>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbSet.Where(u => u.EmployeeId == employeeId).ToListAsync();
            return result;      
        }

        // Kiểm tra có BusyTime trùng lịch không
        public async Task<bool> ExistsOverlapAsync(int employeeId, byte dayOfWeek, TimeSpan start, TimeSpan end, int? excludeBusyTimeId = null)
        {
            return await _dbSet.AnyAsync(x =>
                x.EmployeeId == employeeId &&
                x.DayOfWeek == dayOfWeek &&
                x.StartTime < end &&
                x.EndTime > start &&
                (excludeBusyTimeId == null || x.BusyTimeId != excludeBusyTimeId)
            );
        }

        // Lấy chi tiết BusyTime theo Id
        public async Task<BusyTime> GetByIdAsync(int busyTimeId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(u => u.BusyTimeId == busyTimeId);
            return result;
        }
    }
}
