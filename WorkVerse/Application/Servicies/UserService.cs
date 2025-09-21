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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> UpdateStatusAsync(int userId, string newStatus)
        {
            try
            {
                var result = await _unitOfWork.User.UpdateStatusAsync(userId, newStatus);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogWarning("UpdateStatusAsync: User with id {UserId} not found or status not updated.", userId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateStatusAsync: Error updating status for user id {UserId}", userId);
                throw;
            }
        }
    }
}
