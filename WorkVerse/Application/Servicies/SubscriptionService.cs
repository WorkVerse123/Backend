using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriptionService> _logger;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper, ILogger<SubscriptionService> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<SubscriptionPlanDTOResponse>> GetAll()
        {
            try
            {
                var plans = await _subscriptionRepository.GetAll();
                var result = _mapper.Map<IEnumerable<SubscriptionPlanDTOResponse>>(plans);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubscriptionService.GetAll");
                throw;
            }
        }

        public async Task<SubscriptionPlanDTOResponse?> GetByUser(int id)
        {
            try
            {
                var plan = await _subscriptionRepository.GetByUser(id);
                if (plan == null)
                    return null;

                var dto = _mapper.Map<SubscriptionPlanDTOResponse>(plan);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubscriptionService.GetByUser for userId {UserId}", id);
                throw;
            }
        }

        public async Task<bool> RegisterSubsciption(int userId, int planId)
        {
            try
            {
                var result = await _subscriptionRepository.RegisterSubsciption(userId, planId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubscriptionService.RegisterSubsciption for userId {UserId}, planId {PlanId}", userId, planId);
                throw;
            }
        }
    }
}
