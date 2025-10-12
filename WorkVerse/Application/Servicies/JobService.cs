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
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<JobService> _logger;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<JobService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<JobListDTOResponse> GetJobListAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Job.GetAllJobsAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<JobSummaryDTO>>(pagedData);

                return new JobListDTOResponse
                {
                    Jobs = mapped,
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
                _logger.LogError(ex, "Error retrieving Jobs");
                throw;
            }
        }
        public async Task<JobDetailsDTOResponse?> GetJobByIdAsync(int jobId)
        {
            try
            {
                var entity = await _unitOfWork.Job.GetByIdAsync(jobId);
                return _mapper.Map<JobDetailsDTOResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Job with ID: {Id}", jobId);
                throw;
            }
        }
        public async Task<JobListDTOResponse> GetJobsByEmployerIdAsync(int employerId, int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Job.GetJobsByEmployerIdAsync(employerId))
                            .AsQueryable();
                var totalRecords = query.Count();
                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                var mapped = _mapper.Map<List<JobSummaryDTO>>(pagedData);
                foreach (var job in mapped)
                {
                    job.EmployeeApplyCount = await _unitOfWork.Job.CountJobApply(job.JobId);
                }
                return new JobListDTOResponse
                {
                    EmployerId = employerId,
                    Jobs = mapped,
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
                _logger.LogError(ex, "Error retrieving Jobs for Employer ID: {EmployerId}", employerId);
                throw;
            }
        }
        public async Task AddJobAsync(JobDTORequest request)
        {
            try
            {
                var job = _mapper.Map<Job>(request);
                await _unitOfWork.Job.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.Job.AddJobCategory(job.JobId, request.CategoryIds);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new Job");
                throw;
            }
        }

        public async Task<bool> UpdateJobAsync(int jobId, JobDTORequest request)
        {
            try
            {
                var exists = await _unitOfWork.Job.ExistsByJobIdAsync(jobId);
                if (!exists)
                {
                    return false;
                }
                var job = _mapper.Map<Job>(request);
                job.JobId = jobId;
                _unitOfWork.Job.Update(job);
                await _unitOfWork.Job.RemoveJobCategory(jobId);
                await _unitOfWork.Job.AddJobCategory(jobId, request.CategoryIds);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Job with ID: {Id}", jobId);
                throw;
            }
        }
        public async Task<bool> UpdateJobStatusAsync(int jobId, string newStatus)
        {
            try
            {
                var job = await _unitOfWork.Job.GetByIdAsync(jobId);
                if (job == null)
                {
                    return false;
                }
                job.Status = newStatus;
                _unitOfWork.Job.Update(job);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status of Job with ID: {Id}", jobId);
                throw;
            }
        }

        public async Task<IEnumerable<JobAIDTOResponse>> SearchJobByAIResult(JobQuery jobQuery)
        {
            try
            {
                var jobs = await _unitOfWork.Job.SearchJobByAIResult(jobQuery) ?? Enumerable.Empty<Job>();
                return _mapper.Map<IEnumerable<JobAIDTOResponse>>(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching jobs by AI result");
                throw;
            }
        }

        public async Task<IEnumerable<JobWithEmployerAIDTOResponse>> SearchJobByEmployerAIResult(JobQuery jobQuery, EmployerQuery employerQuery)
        {
            try
            {
                var jobs = await _unitOfWork.Job.SearchJobByEmployerAIResult(jobQuery, employerQuery) ?? Enumerable.Empty<Job>(); ;
                return _mapper.Map<IEnumerable<JobWithEmployerAIDTOResponse>>(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching jobs with employer by AI result");
                throw;
            }
        }

		public async Task<JobListDTOResponse> GetJobsFilter(JobFilterRequest filter, int pageNumber, int pageSize)
		{
			var query = (await _unitOfWork.Job.GetAllJobsAsync())
							.AsQueryable();

			// Search chung
			if (!string.IsNullOrWhiteSpace(filter.Search))
			{
				string search = filter.Search.Trim().ToLower();
				query = query.Where(j =>
					(!string.IsNullOrEmpty(j.Title) && j.Title.ToLower().Contains(search)) ||
					(!string.IsNullOrEmpty(j.Description) && j.Description.ToLower().Contains(search)) ||
					(!string.IsNullOrEmpty(j.Requirements) && j.Requirements.ToLower().Contains(search)) ||
					(!string.IsNullOrEmpty(j.Location) && j.Location.ToLower().Contains(search))
				);
			}


			if (filter.CategoryId != null && filter.CategoryId.Any())
			{
				query = query.Where(j => j.JobCategoryMappings.Any(jc => filter.CategoryId.Contains(jc.CategoryId)));
			}


			// Salary
			if (filter.SalaryMin.HasValue)
				query = query.Where(j => j.SalaryMax >= filter.SalaryMin.Value);
			if (filter.SalaryMax.HasValue)
				query = query.Where(j => j.SalaryMin <= filter.SalaryMax.Value);

			if (filter.JobTime.HasValue)
			{
				string jobTimeStr = JobValidationHelper.GetDescription(filter.JobTime.Value);
				query = query.Where(j => j.JobTime == jobTimeStr);
			}


			// Luôn ưu tiên job IsPriority = true trước
			query = query.OrderByDescending(j => j.IsPriority).ThenByDescending(j => j.CreatedAt);

			// Tính tổng số record sau filter
			var totalRecords =  query.Count();

			// Phân trang
			var pagedData = query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			// Map sang DTO nếu cần
			var mapped = _mapper.Map<List<JobSummaryDTO>>(pagedData);
			foreach (var jobDto in mapped)
			{
				jobDto.EmployeeApplyCount = await _unitOfWork.Job.CountJobApply(jobDto.JobId);
			}

			return new JobListDTOResponse
			{
				Jobs = mapped,
				Paging = new PaginatedResponse
				{
					Page = pageNumber,
					PageSize = pageSize,
					TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
				}
			};
		}

        public async Task<PaginationResult<List<JobDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var jobs = await _unitOfWork.Job.GetAllAsync(
                    order: q => q.OrderBy(j => j.JobId),
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                var jobDTOs = _mapper.Map<List<JobDTOResponse>>(jobs.Data);

                return new PaginationResult<List<JobDTOResponse>>(
                    jobDTOs,
                    jobs.TotalRecords,
                    jobs.PageIndex,
                    jobs.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync: Error retrieving jobs");
                throw;
            }
        }

        public async Task<JobDTOResponse> UpdateAsync(JobDTORequest entity)
        {
            try
            {
                var job = await _unitOfWork.Job.GetByIdAsync(entity.JobId);
                if (job == null)
                {
                    _logger.LogWarning("UpdateAsync: Job with id {JobId} not found.", entity.JobId);
                    throw new KeyNotFoundException($"Job with id {entity.JobId} not found.");
                }
                _mapper.Map(entity, job);
                _unitOfWork.Job.Update(job);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<JobDTOResponse>(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync: Error updating job with id {JobId}", entity.JobId);
                throw;
            }
        }
    }
}
