using Application.DTOs.Common;
using Application.DTOs.Request;
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

        public async Task<bool> ExistsByUserPhoneAsync(string phone)
        {
            try
            {
                var phoneExist = await _unitOfWork.User.ExistsByUserPhoneAsync(phone);
                return phoneExist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving users");
                throw;
            }
        }

        public async Task<PaginationResult<List<UserDTORespone>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var users = await _unitOfWork.User.GetAllAsync(
                    order: q => q.OrderBy(u => u.UserId),
                    pageIndex: pageIndex,
                    pageSize: pageSize);
                var userDTOs = _mapper.Map<List<UserDTORespone>>(users.Data);
                return new PaginationResult<List<UserDTORespone>>(userDTOs, users.TotalRecords, users.PageIndex, users.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving users");
                throw;
            }
        }

        public async Task<UserDTORespone> UpdateAsync(UserUpdateDTORequest entity)
        {
            try
            {
                var user = await _unitOfWork.User.GetAsync(entity.UserId);
                if (user == null)
                {
                    _logger.LogWarning("UpdateAsync: User with id {UserId} not found.", entity.UserId);
                    throw new KeyNotFoundException($"User with id {entity.UserId} not found.");
                }
                _mapper.Map(entity, user);
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<UserDTORespone>(user);
            }
            catch (Exception)
            {
                _logger.LogError("UpdateAsync: Error updating user with id {UserId}", entity.UserId);
                throw;
            }

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
