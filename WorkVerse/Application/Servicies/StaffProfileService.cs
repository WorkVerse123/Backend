using Application.DTOs.Common;
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

        public async Task<StaffProfileDTOResponse> Create(StaffProfileDTORequest request)
        {
            try
            {
             var entity = _mapper.Map<StaffProfile>(request);
                await _unitOfWork.StaffProfile.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<StaffProfileDTOResponse>(entity);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating StaffProfile");
                throw;
            }
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating StaffProfile with id {Id}", staffProfile.StaffId);
                throw;
            }
        }

        public async Task<PaginationResult<List<StaffProfileDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var staffProfiles = await _unitOfWork.StaffProfile.GetAllAsync(
                    order: q => q.OrderBy(s => s.StaffId),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                var staffDTOs = _mapper.Map<List<StaffProfileDTOResponse>>(staffProfiles.Data);

                return new PaginationResult<List<StaffProfileDTOResponse>>(
                    staffDTOs,
                    staffProfiles.TotalRecords,
                    staffProfiles.PageIndex,
                    staffProfiles.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving staff profiles");
                throw;
            }
        }

        public async Task<StaffProfileDTOResponse> UpdateAsync(StaffProfileDTORequest entity)
        {
            try
            {
                var profile = await _unitOfWork.StaffProfile.GetAsync(entity.StaffId);
                if (profile == null)
                {
                    _logger.LogWarning("UpdateAsync: StaffProfile with id {StaffId} not found.", entity.StaffId);
                    throw new KeyNotFoundException($"StaffProfile with id {entity.StaffId} not found.");
                }
                _mapper.Map(entity, profile);
                _unitOfWork.StaffProfile.Update(profile);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<StaffProfileDTOResponse>(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync: Error updating staff profile with id {StaffId}", entity.StaffId);
                throw;
            }
        }
    }
}
