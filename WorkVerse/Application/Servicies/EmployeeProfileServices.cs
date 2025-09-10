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
    public class EmployeeProfileServices : IEmployeeProfileServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeProfileServices> _logger;

        public EmployeeProfileServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeProfileServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeProfileDTOResponse> CreateProfileAsync(int userId, EmployeeProfileDTORequest employeeProfile)
        {
            try
            {
                if (employeeProfile == null)
                {
                    _logger.LogWarning("Attempted to add a null employee profile");
                    throw new ArgumentNullException(nameof(employeeProfile), "Profile cannot be null");
                }

                var existsProfile = await _unitOfWork.EmployeeProfile.GetByUserIdAsync(userId);
                if (existsProfile != null)
                {
                    throw new InvalidOperationException($"Profile already exists for User {userId}");
                }

                // Map DTO request sang entity
                var entity = _mapper.Map<EmployeeProfile>(employeeProfile);
                entity.UserId = userId;
                // Add vào DB
                await _unitOfWork.EmployeeProfile.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                // Map entity vừa tạo sang DTO response
                return _mapper.Map<EmployeeProfileDTOResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee profile for UserId: {UserId}", userId);
                throw;
            }
        }


        public async Task<EmployeeProfileDTOResponse?> GetByIdAsync(int employeeId)
        {
            try
            {
                var entity = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                return _mapper.Map<EmployeeProfileDTOResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving EmployeeProfile with ID: {Id}", employeeId);
                throw;
            }
        }

        public async Task<EmployeeProfileDTOResponse> UpdateProfileAsync(int employeeId, EmployeeProfileDTORequest employeeProfile)
        {
            try
            {
                if (employeeProfile == null)
                {
                    _logger.LogWarning("Attempted to update a null profile with ID: {Id}", employeeId);
                    throw new ArgumentNullException(nameof(employeeProfile), "Profile cannot be null");
                }

                var existingProfile = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (existingProfile == null)
                {
                    _logger.LogWarning("Profile with ID {Id} not found", employeeId);
                    throw new KeyNotFoundException($"Profile with ID {employeeId} not found");
                }

                // Map dữ liệu request vào entity có sẵn
                _mapper.Map(employeeProfile, existingProfile);

                _unitOfWork.EmployeeProfile.Update(existingProfile);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<EmployeeProfileDTOResponse>(existingProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile with ID: {Id}", employeeId);
                throw;
            }
        }

        public async Task<EmployeeProfileDTOResponse?> GetByUserIdAsync(int userId)
        {
            try
            {
                var entity = await _unitOfWork.EmployeeProfile.GetByUserIdAsync(userId);
                return _mapper.Map<EmployeeProfileDTOResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving EmployeeProfile with UserId: {Id}", userId);
                throw;
            }
        }

        public async Task<CandidateDTOResponse> GetAllCandidatesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.EmployeeProfile.GetAllCandidatesAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<CandidateItemDTO>>(pagedData);

                return new CandidateDTOResponse
                {
                    Candidates = mapped,
                    Paging = new PaginatedResponse
                    {
                        Page = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Candidates for Job");
                throw;
            }
        }
    }
}
