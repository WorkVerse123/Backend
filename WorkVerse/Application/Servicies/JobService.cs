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

        public async Task<JobDTOResponse> GetAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Job.GetAllAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<JobItemDTO>>(pagedData);

                return new JobDTOResponse
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
        public async Task<JobItemDetailDTO?> GetByIdAsync(int jobId)
        {
            try
            {
                var entity = await _unitOfWork.Job.GetByIdAsync(jobId);
                return _mapper.Map<JobItemDetailDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Job with ID: {Id}", jobId);
                throw;
            }
        }
        public async Task<JobDTOResponse> GetByEmployerIdAsync(int employerId, int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Job.GetByEmployerIdAsync(employerId))
                            .AsQueryable();
                var totalRecords = query.Count();
                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                var mapped = _mapper.Map<List<JobItemDTO>>(pagedData);
                return new JobDTOResponse
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
                _logger.LogError(ex, "Error retrieving Jobs for Employer ID: {EmployerId}", employerId);
                throw;
            }
        }
        public async Task AddAsyne(JobDTORequest request)
        {
            try
            {
                var job = _mapper.Map<Job>(request);
                await _unitOfWork.Job.AddAsync(job);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new Job");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int jobId, JobDTORequest request)
        {
            try
            {
                var exists = await _unitOfWork.Job.ExistsAsync(jobId);
                if (!exists)
                {
                    return false;
                }
                var job = _mapper.Map<Job>(request);
                job.JobId = jobId;
                _unitOfWork.Job.Update(job);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Job with ID: {Id}", jobId);
                throw;
            }
        }
        public async Task<bool> ChangeStatusAsynce(int jobId, string newStatus)
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
    }
}
