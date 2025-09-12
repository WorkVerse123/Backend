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

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
