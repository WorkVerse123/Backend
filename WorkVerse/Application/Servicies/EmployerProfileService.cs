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
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployerProfileService> _logger;

        public EmployerProfileService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployerProfileService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CreateEmployerProfileAsync(EmployerProfileDTORequest request)
        {
            try
            {
                var existsUser = await _unitOfWork.User.ExistByIdAsync(request.UserId);
                if (!existsUser)
                {
                    throw new InvalidOperationException($"User not exists for User {request.UserId}");
                }
                var existingProfile = await _unitOfWork.EmployerProfile.ExistsByUserIdAsync(request.UserId);
                if (existingProfile)
                {
                    throw new InvalidOperationException($"Employer profile already exists for User {request.UserId}");
                }

                var entity = _mapper.Map<EmployerProfile>(request);

                await _unitOfWork.EmployerProfile.AddAsync(entity);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employer profile for UserId: {UserId}", request.UserId);
                throw;
            }
        }


        public async Task<ListEmployerProfileDTOResponse> GetAllEmployersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.EmployerProfile.GetAllEmployersAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<CompanyItemDTO>>(pagedData);

                return new ListEmployerProfileDTOResponse
                {
                    Companies = mapped,
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
                _logger.LogError(ex, "Error retrieving Companies");
                throw;
            }
        }

        public async Task<EmployerProfileDTOResponse> GetEmployerProfileByIdAsync(int id)
        {
            try
            {
                var employerProfile = await _unitOfWork.EmployerProfile.GetByIdAsync(id);

                if (employerProfile == null)
                {
                    throw new KeyNotFoundException($"Employer profile with ID {id} not found.");
                }

                return _mapper.Map<EmployerProfileDTOResponse>(employerProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employer profile with ID: {Id}", id);
                throw;
            }
        }

        public  async Task<int?> GetEmployerIdByUserIdAsync(int userId)
        {
            var user = await _unitOfWork.EmployerProfile.GetByUserIdAsync(userId);
            return user?.EmployerId;
        }

        public async Task<bool> UpdateEmployerProfileAsync(int id, EmployerProfileDTORequest request)
        {
            try
            {
                // Validate request
                var (isValid, errorMessage) = Helper.EmployerProfileValidationHelper.ValidateEmployerProfilePostRequest(request);
                if (!isValid)
                {
                    throw new ArgumentException(errorMessage);
                }

                // Get existing profile
                var existingProfile = await _unitOfWork.EmployerProfile.GetByIdAsync(id);
                var employerType = await _unitOfWork.EmployerType.GetAsync(request.EmployerType);
                if (existingProfile == null)
                {
                    throw new KeyNotFoundException($"Employer profile with ID {id} not found.");
                }

                // Update fields
                existingProfile.CompanyName = request.CompanyName;
                existingProfile.EmployerType = employerType;
                existingProfile.Address = request.Address;
                existingProfile.WebsiteUrl = request.WebsiteUrl;
                existingProfile.LogoUrl = request.LogoUrl;
                existingProfile.DateEstablish = request.DateEstablished;
                existingProfile.Description = request.Description;

                // Save changes
                _unitOfWork.EmployerProfile.Update(existingProfile);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employer profile with ID: {Id}", id);
                throw;
            }
        }
    }
}
