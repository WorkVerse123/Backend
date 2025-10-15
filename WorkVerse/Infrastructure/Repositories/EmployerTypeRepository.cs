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
    public class EmployerTypeRepository : GenericRepository<EmployerType>, IEmployerTypeRepository
    {
        public EmployerTypeRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public Task<List<EmployerType>> GetAllEmployerTypesAsync()
        {
            return Task.FromResult(_dbSet.ToList());
        }
        public async Task<bool> GetByIdAsync(int employerTypeId)
        {
            return await _context.EmployerTypes.AnyAsync(x => x.EmployerTypeId == employerTypeId);
        }

    }
}
