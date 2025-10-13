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
    public class UserSubscriptionRepository : GenericRepository<UserSubscription>, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(WorkVerseDBContext context) : base(context)
        {
        }

        public async Task<bool> UpdateStatusAsync(int userId, bool isActive)
        {
            UserSubscription subscription = await _dbSet.Where(us => us.UserId == userId).FirstOrDefaultAsync();
            if (subscription == null )
            {
                return false;
            }
            subscription.IsActive = isActive;
            _dbSet.Update(subscription);
            return true;
        }
    }
}
