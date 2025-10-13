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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {

        public RoleRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByIdAsync(int roleId)
        {
            return await _dbSet.AnyAsync(r => r.RoleId == roleId);
        }

       
    }
}
