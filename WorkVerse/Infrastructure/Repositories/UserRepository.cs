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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(string email, string phoneNumber)
        {
            return await _dbSet.AnyAsync(u =>
                u.Email == email || u.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> ExistByIdAsync(int userId)
        {
            return await _dbSet.AnyAsync(u => u.UserId == userId);
        }

        public async Task<string?> GetUserFullNameByIdAsync(int id)
        {
            return await _dbSet
                .Where(u => u.UserId == id)
                .Select(u =>
                    u.EmployeeProfile != null
                        ? u.EmployeeProfile.FullName
                        : (u.EmployerProfile != null
                            ? u.EmployerProfile.CompanyName
                            : null))
                .FirstOrDefaultAsync();
        }
    }
}
