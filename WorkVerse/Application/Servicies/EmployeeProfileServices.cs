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
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<EmployeeProfileDTOResponse> CreateEmployeeProfileAsync(int userId, EmployeeProfileDTORequest employeeProfile)
        {
            try
            {
                if (employeeProfile == null)
                {
                    _logger.LogWarning("Attempted to add a null employee profile");
                    throw new ArgumentNullException(nameof(employeeProfile), "Profile cannot be null");
                }
                var existsUser = await _unitOfWork.User.ExistByIdAsync(userId);
                if (!existsUser)
                {
                    throw new InvalidOperationException($"User not exists for User {userId}");
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


        public async Task<EmployeeProfileDTOResponse?> GetEmployeeProfileByIdAsync(int employeeId)
        {
            try
            {
                var entity = await _unitOfWork.EmployeeProfile.GetByEmployeeIdAsync(employeeId);
                return _mapper.Map<EmployeeProfileDTOResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving EmployeeProfile with ID: {Id}", employeeId);
                throw;
            }
        }

        public async Task<EmployeeProfileDTOResponse> UpdateEmployeeProfileAsync(int employeeId, EmployeeProfileDTORequest employeeProfile)
        {
            try
            {
                if (employeeProfile == null)
                {
                    _logger.LogWarning("Attempted to update a null profile with ID: {Id}", employeeId);
                    throw new ArgumentNullException(nameof(employeeProfile), "Profile cannot be null");
                }

                var existingProfile = await _unitOfWork.EmployeeProfile.GetByEmployeeIdAsync(employeeId);
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

        public async Task<EmployeeProfileDTOResponse?> GetEmployeeProfileByUserIdAsync(int userId)
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

        public async Task<CandidateListDTOResponse> GetEmployeeListAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.EmployeeProfile.GetAllPublicEmployeeAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(c => c.IsPriority)
                    .ToList();

                var mapped = _mapper.Map<List<CandidateItemDTO>>(pagedData);

                return new CandidateListDTOResponse
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

        public async Task<EmployeeDashboardDTOResponse> GetEmployeeDashBoardAsync(int employeeId, int pageNumber, int pageSize)
        {
            try
            {
                var existingProfile = await _unitOfWork.EmployeeProfile.GetByEmployeeIdAsync(employeeId);
                if (existingProfile == null)
                {
                    throw new KeyNotFoundException($"Profile with ID {employeeId} not found");
                }
                // Lấy thống kê
                var totalApplications = await _unitOfWork.Application.CountApplicationsByEmployeeIdAsync(employeeId);
                var totalFavorites = await _unitOfWork.Bookmark.CountBookmarkJobsByEmployeeIdAsync(employeeId);
                var totalNotifications = await _unitOfWork.Notification.CountNotificationsByEmployeeIdAsync(employeeId);

                // Lấy danh sách ứng tuyển 
                var query = (await _unitOfWork.Application.GetAppliEmployerByEmployeeIdAsync(employeeId))
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


                // Map sang DTO
                var applicationDtos = _mapper.Map<List<EmployeeApplicationDTO>>(pagedData);

                // Trả về response
                return new EmployeeDashboardDTOResponse
                {

                    Stats = new List<DashboardStat>
                    {
                    new() { Label = "Công việc đã ứng tuyển", Value = totalApplications },
                    new() { Label = "Công việc yêu thích", Value = totalFavorites },
                    new() { Label = "Thông báo", Value = totalNotifications }
                    },
                    Applications = applicationDtos,
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
                _logger.LogError(ex, "Error when getting dashboard for employee {EmployeeId}", employeeId);
                throw;
            }
        }


        public async Task<int?> GetEmployeeIdByUserIdAsync(int userId)
        {
            var user = await  _unitOfWork.EmployeeProfile.GetByUserIdAsync(userId);
            return user?.EmployeeId;
        }

        public async Task<IEnumerable<EmployeeAIDTOResponse>> SearchEmployeeByAIResult(EmployeeQuery employeeQuery)
        {
            try
            {
                var employees = await _unitOfWork.EmployeeProfile.SearchEmployeeByAIResult(employeeQuery) ?? Enumerable.Empty<EmployeeProfile>();
                return _mapper.Map<IEnumerable<EmployeeAIDTOResponse>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching employers by AI result");
                throw;
            }
        }

        public async Task<bool> UpdatePriority(int employeeId, bool isPriority)
        {
            try
            {
                var existingProfile = await _unitOfWork.EmployeeProfile.GetByEmployeeIdAsync(employeeId);
                if (existingProfile == null)
                {
                    return false;
                }
                var result = await _unitOfWork.EmployeeProfile.UpdatePriority(employeeId, isPriority);
                if (result)
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating priority for employee {EmployeeId}", employeeId);
                throw;
            }
        }

        public async Task<PaginationResult<List<EmployeeProfileDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var employees = await _unitOfWork.EmployeeProfile.GetAllAsync(
                    order: q => q.OrderBy(e => e.EmployeeId),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                var employeeDTOs = _mapper.Map<List<EmployeeProfileDTOResponse>>(employees.Data);

                return new PaginationResult<List<EmployeeProfileDTOResponse>>(
                    employeeDTOs,
                    employees.TotalRecords,
                    employees.PageIndex,
                    employees.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving employee profiles");
                throw;
            }
        }

        public async Task<EmployeeProfileDTOResponse> UpdateAsync(EmployeeProfileUpdateDTORequest entity)
        {
            try
            {
                var profile = await _unitOfWork.EmployeeProfile.GetByEmployeeIdAsync(entity.EmployeeId);
                if (profile == null)
                {
                    _logger.LogWarning("UpdateAsync: EmployeeProfile with id {EmployeeId} not found.", entity.EmployeeId);
                    throw new KeyNotFoundException($"EmployeeProfile with id {entity.EmployeeId} not found.");
                }
                _mapper.Map(entity, profile);
                _unitOfWork.EmployeeProfile.Update(profile);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<EmployeeProfileDTOResponse>(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync: Error updating employee profile with id {EmployeeId}", entity.EmployeeId);
                throw;
            }
        }
        }

        public async Task<CandidateListDTOResponse> GetEmployeesFilter(EmployeeFilterRequest filter, int pageNumber, int pageSize)
        {
            var query = (await _unitOfWork.EmployeeProfile.GetAllPublicEmployeeAsync())
                            .AsQueryable();

            // 1. Search chung (tìm theo tên, kỹ năng, mô tả, học vấn, kinh nghiệm)
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string search = filter.Search.Trim().ToLower();
                query = query.Where(e =>
                    (!string.IsNullOrEmpty(e.FullName) && e.FullName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.Skills) && e.Skills.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.Bio) && e.Bio.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.Education) && e.Education.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.WorkExperience) && e.WorkExperience.ToLower().Contains(search))
                );
            }

            // 2. Lọc theo địa chỉ / tỉnh thành
            if (filter.EmployeeLocation != null && filter.EmployeeLocation.Any())
            {
                var provinceNames = filter.EmployeeLocation
                    .Select(p => JobValidationHelper.GetDescription((EmployeeLocation)p).ToLower())
                    .ToList();

                query = query.Where(e =>
                    !string.IsNullOrEmpty(e.Address) &&
                    provinceNames.Any(prov => e.Address.ToLower().Contains(prov))
                );
            }

            // 3. Lọc theo học vấn (text search mềm)
            if (filter.EmployeeEducation != null && filter.EmployeeEducation.Any())
            {
                query = query.Where(e => EducationMatchingHelper.MatchesEducation(e, filter.EmployeeEducation));
            }

            //  4. Lọc theo giới tính
            if (filter.Gender != null && filter.Gender.Any())
            {
                var genderList = filter.Gender.Select(g =>
                {
                    return g switch
                    {
                        1 => "male",
                        2 => "female",
                        3 => "others",
                        _ => string.Empty
                    };
                }).Where(g => !string.IsNullOrEmpty(g)).ToList();

                query = query.Where(e =>
                    !string.IsNullOrEmpty(e.Gender) &&
                    genderList.Any(g => e.Gender.ToLower().Equals(g.ToLower()))
                );
            }

            // 5. Tổng số record
            var totalRecords = query.Count();

            var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(c => c.IsPriority)
                    .ToList();

            // 7. Map sang DTO
            var mapped = _mapper.Map<List<CandidateItemDTO>>(pagedData);

            return new CandidateListDTOResponse
            {
                Candidates = mapped,
                Paging = new PaginatedResponse
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                }
            };
        }

    }
}
