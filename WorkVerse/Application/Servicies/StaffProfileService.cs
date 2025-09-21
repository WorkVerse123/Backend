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
    public class StaffProfileService : IStaffProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffProfileService> _logger;
        public StaffProfileService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StaffProfileService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<StaffProfileDTOResponse> Get(int id)
        {
            try
            {
                var staffProfile = await _unitOfWork.StaffProfile.GetAsync(id);

                if (staffProfile == null)
                {
                    _logger.LogWarning("StaffProfile with id {Id} not found.", id);
                    return null!;
                }

                // Map StaffProfile entity to DTO
                var staffProfileEntity = await _unitOfWork.StaffProfile.GetAsync(id);
                var dto = _mapper.Map<StaffProfileDTOResponse>(staffProfileEntity);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting StaffProfile with id {Id}", id);
                throw;
            }
        }

        public async Task<StaffProfileDTOResponse> Update(StaffProfileDTORequest staffProfile)
        {
            try
            {
                bool exists = await _unitOfWork.StaffProfile.IsExist(staffProfile.StaffId);
                {
                    if (!exists)
                    {
                        _logger.LogWarning("StaffProfile with id {Id} does not exist.", staffProfile.StaffId);
                        return null!;
                    }

                    var entity = _mapper.Map<StaffProfile>(staffProfile);


                _unitOfWork.StaffProfile.Update(entity);

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<StaffProfileDTOResponse>(entity);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating StaffProfile with id {Id}", staffProfile.StaffId);
                throw;
            }
        }
    }
}
