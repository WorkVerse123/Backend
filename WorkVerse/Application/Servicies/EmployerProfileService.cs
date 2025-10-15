using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
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
                var phoneExist = await _unitOfWork.EmployerProfile.ExistsByContactPhoneAsync(request.ContactPhone);
                if (phoneExist)
                {
                    throw new InvalidOperationException($"Phone already exists in the system");
                }
                var emailExist = await _unitOfWork.EmployerProfile.ExistsByContactEmailAsync(request.ContactEmail);
                if (emailExist)
                {
                    throw new InvalidOperationException($"Email already exists in the system");
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
                    .OrderByDescending(c => c.IsPriority)
                    .ThenByDescending(c => c.DateEstablish)
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
                var employerType = await _unitOfWork.EmployerType.GetAsync(request.EmployerTypeId);
                if (existingProfile == null)
                {
                    throw new KeyNotFoundException($"Employer profile with ID {id} not found.");
                }

                // Update fields
                existingProfile.CompanyName = request.CompanyName;
                existingProfile.EmployerTypeId = request.EmployerTypeId;
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

        public async Task<IEnumerable<EmployerAIDTOResponse>> SearchEmployerByAIResult(EmployerQuery employerQuery)
        {
            try
            {
                var employers = await _unitOfWork.EmployerProfile.SearchEmployerByAIResult(employerQuery) ?? Enumerable.Empty<EmployerProfile>();
                return _mapper.Map<IEnumerable<EmployerAIDTOResponse>>(employers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching employers by AI result");
                throw;
            }
        }

        public async Task<bool> UpdateUserByEmployerId(int employerId, string newPhone, string newEmail)
        {
            await _unitOfWork.User.UpdateUserByEmployerId(employerId, newPhone, newEmail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ListEmployerProfileFilterDTOResponse> GetEmployersFilter(EmployerFilterRequest filter, int pageNumber, int pageSize)
        {
            var query = (await _unitOfWork.EmployerProfile.GetAllEmployersAsync())
                            .AsQueryable();

            // Search chung
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string search = filter.Search.Trim().ToLower();
                query = query.Where(e =>
                    (!string.IsNullOrEmpty(e.CompanyName) && e.CompanyName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.Description) && e.Description.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.ContactEmail) && e.ContactEmail.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.ContactPhone) && e.ContactPhone.ToLower().Contains(search))
                );
            }

            // Loại hình (công ty / cá nhân)
            if (filter.EmployerTypeId != null && filter.EmployerTypeId.Any())
            {
                query = query.Where(e => filter.EmployerTypeId.Contains(e.EmployerTypeId));
            }

            // Khu vực (34 tỉnh/thành)
            if (filter.Locations != null && filter.Locations.Any())
            {
                var provinceNames = filter.Locations
                    .Select(p => JobValidationHelper.GetDescription((EmployerFilterType)p).ToLower()) // description lowercase
                    .ToList();

                query = query.Where(e =>
                    !string.IsNullOrEmpty(e.Address) &&
                    provinceNames.Any(prov => e.Address.ToLower().Contains(prov)) // address lowercase
                );
            }


            // Tổng số record
            var totalRecords = query.Count();

            // Phân trang
            var pagedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .OrderByDescending(c => c.IsPriority)
                .ThenByDescending(c => c.DateEstablish);

            // Map sang DTO
            var mapped = _mapper.Map<List<EmployerProfileDTOResponse>>(pagedData);

            return new ListEmployerProfileFilterDTOResponse
            {
                Employers = mapped,
                Paging = new PaginatedResponse
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                }
            };
        }

        public async Task<PaginationResult<List<EmployerProfileDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var employers = await _unitOfWork.EmployerProfile.GetAllAsync(
                    order: q => q.OrderBy(e => e.EmployerId),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                var employerDTOs = _mapper.Map<List<EmployerProfileDTOResponse>>(employers.Data);

                return new PaginationResult<List<EmployerProfileDTOResponse>>(
                    employerDTOs,
                    employers.TotalRecords,
                    employers.PageIndex,
                    employers.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving employer profiles");
                throw;
            }
        }

        public async Task<EmployerProfileDTOResponse> UpdateAsync(EmployerProfileDTORequest entity)
        {
            try
            {
                var profile = await _unitOfWork.EmployerProfile.GetByIdAsync(entity.EmployerId);
                if (profile == null)
                {
                    _logger.LogWarning("UpdateAsync: EmployerProfile with id {EmployerId} not found.", entity.EmployerId);
                    throw new KeyNotFoundException($"EmployerProfile with id {entity.EmployerId} not found.");
                }

                // Map các trường từ DTO sang entity, trừ EmployerType (xử lý riêng)
                _mapper.Map(entity, profile);

                // Nếu có thay đổi loại hình, cập nhật lại reference
                if (profile.EmployerTypeId != entity.EmployerTypeId)
                {
                    var employerType = await _unitOfWork.EmployerType.GetAsync(entity.EmployerTypeId);
                    profile.EmployerType = employerType;
                }

                _unitOfWork.EmployerProfile.Update(profile);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<EmployerProfileDTOResponse>(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync: Error updating employer profile with id {EmployerId}", entity.EmployerId);
                throw;
            }
        }
    }
}
