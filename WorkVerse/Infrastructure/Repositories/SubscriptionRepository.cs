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
    public class SubscriptionRepository : GenericRepository<SubscriptionPlan>, ISubscriptionRepository
    {
        public SubscriptionRepository(WorkVerseDBContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<SubscriptionPlan>> GetAll()
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }

        public async Task<SubscriptionPlan?> GetByUser(int id)
        {
            // Lấy subscription hiện tại của user (ưu tiên cái mới nhất nếu có nhiều)
            var userSubscription = await _context.UserSubscriptions
                .Where(us => us.UserId == id)
                .OrderByDescending(us => us.UserSubscriptionId)
                .Include(us => us.Plan)
                .FirstOrDefaultAsync();

            return userSubscription?.Plan;
        }

        public async Task<bool> RegisterSubsciption(int userId, int planId)
        {
            // Kiểm tra user đã đăng ký plan này chưa
            var exists = await _context.UserSubscriptions
                .AnyAsync(us => us.UserId == userId && us.PlanId == planId);

            if (exists)
                return false;

            var userSubscription = new UserSubscription
            {
                UserId = userId,
                PlanId = planId
            };

            await _context.UserSubscriptions.AddAsync(userSubscription);
            return true;
        }
    }
}
