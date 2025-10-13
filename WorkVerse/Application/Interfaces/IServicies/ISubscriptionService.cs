using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionPlanDTOResponse>> GetAll();
        Task<SubscriptionPlanDTOResponse?> GetByUser(int id);
        Task<bool> RegisterSubsciption(int userId, int planId);
    }
}
