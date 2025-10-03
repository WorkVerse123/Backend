using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface ISubscriptionRepository : IGenericRepository<SubscriptionPlan>
    {
        Task<IEnumerable<SubscriptionPlan>> GetAll();
        Task<SubscriptionPlan?> GetByUser(int id);
        Task<bool> RegisterSubsciption(int userId, int planId);
    }
}
