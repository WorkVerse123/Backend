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
        private readonly WorkVerseDBContext _dbContext;
        public BusyTimeRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BusyTime>> GetByEmployeeIdAsync(int employeeId)
        {
            var result = await _dbContext.BusyTimes.Where(u => u.EmployeeId == employeeId).ToListAsync();
            return result;
        }
    }
}
