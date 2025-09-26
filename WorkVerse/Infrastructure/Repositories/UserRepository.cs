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

        public async Task<bool> ExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u =>
                u.Email == email);
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

        public Task<bool> UpdatePasswordAsynce(int userId, string newPasswordHash)
        {
            return Task.Run(async () =>
            {
                var user = await _dbSet.FindAsync(userId);
                if (user == null)
                    return false;
                user.PasswordHash = newPasswordHash;
                return true;
            });
        }

        public Task<bool> IsPremiumAsync(int userId)
        {
            return _context.UserSubscriptions
                .AnyAsync(us => us.UserId == userId && us.IsActive);
        }

        public Task<bool> UpdateStatusAsync(int userId, string newStatus)
        {
            return Task.Run(async () =>
            {
                var user = await _dbSet.FindAsync(userId);
                if (user == null)
                    return false;
                user.Status = newStatus;
                return true;
            });
        }

        public Task<bool> UpdateUserByEmployerId(int employerId, string newPhone, string newEmail)
        {
            return Task.Run(async () =>
            {
                var user = await _dbSet
                    .Include(u => u.EmployerProfile)
                    .FirstOrDefaultAsync(u => u.EmployerProfile != null && u.EmployerProfile.EmployerId == employerId);
                if (user == null)
                    return false;
                user.PhoneNumber = newPhone;
                user.Email = newEmail;
                return true;
            });
        }
    }
}
