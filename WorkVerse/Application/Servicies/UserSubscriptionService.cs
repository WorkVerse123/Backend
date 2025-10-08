using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserSubscriptionService> _logger;
        public UserSubscriptionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserSubscriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserSubsctiptionDTOResponse> AddAsync(PaymentDTORequest request)
        {
            UserSubscriptionDTORequest userSubscriptionDTORequest = new UserSubscriptionDTORequest
            {
                UserId = request.UserId,
                PlanId = request.PlanId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1), // Assuming a 1-month subscription
                IsActive = true
            };

            try
            {
                var entity = _mapper.Map<UserSubscription>(userSubscriptionDTORequest);
                await _unitOfWork.UserSubscription.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<UserSubsctiptionDTOResponse>(entity);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UserSubscriptionService.AddAsync");
                throw;
            }
        }

        public async Task<UserSubsctiptionDTOResponse> UpdateAsynce(UserSubscriptionDTORequest request)
        {
            try
            {

                var entity = _mapper.Map<UserSubscription>(request);

                _unitOfWork.UserSubscription.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<UserSubsctiptionDTOResponse>(entity);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UserSubscriptionService.UpdateAsynce");
                throw;
            }
        }

        public async Task<bool> UpdateStatusAsync(int userId, bool status)
        {
            try
            {
                var result = await _unitOfWork.UserSubscription.UpdateStatusAsync(userId, status);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UserSubscriptionService.UpdateStatusAsync for userId {UserId}", userId);
                throw;
            }
        }
    }
}
