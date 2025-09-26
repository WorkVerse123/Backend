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
    public class StaffProfileRepository : GenericRepository<StaffProfile>,IStaffProfileRepository
    {
        public StaffProfileRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<bool> IsExist(int id)
        {
            return await _dbSet.AnyAsync(sp => sp.StaffId == id);
        }
    }
}
