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
    public class EmployeeRepository : GenericRepository<EmployeeProfile>, IEmployeeRepository
    {
        private readonly WorkVerseDBContext _dbContext;
        public EmployeeRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       

        public async Task<EmployeeProfile?> GetByIdAsync(int employeeId)
        {
            var result = await _dbContext.EmployeeProfiles.FirstOrDefaultAsync(u => u.EmployeeId == employeeId);
            return result;
        }

       
    }
}
