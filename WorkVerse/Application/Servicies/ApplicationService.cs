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
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ApplicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApplicationItemDTO> CreateApplicationAsync(int employeeId, ApplicationDTORequest request)
        {
            try
            {

                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(employeeId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }


                var job = await _unitOfWork.Job.ExistsAsync(request.JobId);
                if (!job)
                {
                    throw new KeyNotFoundException($"Job with ID {request.JobId} not found");
                }

                var exists = await _unitOfWork.Application.ExistsAsync(employeeId, request.JobId);
                if (exists.Status == "pending")
                {
                    throw new InvalidOperationException($"Application already applied and pending for Employee {employeeId} and Job {request.JobId}");
                }


                var application = new Domain.Entities.Application
                {
                    EmployeeId = employeeId,
                    JobId = request.JobId,
                    AppliedAt = DateTime.Now,
                    CoverLetter = request.CoverLetter,
                    Status = "pending"
                };

                await _unitOfWork.Application.AddAsync(application);
                await _unitOfWork.SaveChangesAsync();

                exists = await _unitOfWork.Application.ExistsAsync(employeeId, request.JobId);
                return _mapper.Map<ApplicationItemDTO>(exists);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating application for Employee {EmployeeId} and Job {JobId}", employeeId, request.JobId);
                throw;
            }
        }

        public async Task<ApplicationResponseDTO> GetByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(employeeId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }



                var query = (await _unitOfWork.Application.GetByEmployeeIdAsync(employeeId))
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                if (pagedData == null || !pagedData.Any())
                {
                    return new ApplicationResponseDTO
                    {
                        Applications = new List<ApplicationItemDTO>()
                    };
                }

                var mapped = _mapper.Map<List<ApplicationItemDTO>>(pagedData);

                return new ApplicationResponseDTO
                {
                    Applications = mapped,
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
                _logger.LogError(ex, "Error retrieving Applications with EmployeeId: {Id}", employeeId);
                throw;
            }
        }

        public async Task<ApplicationItemDTO> UpdateApplicationWithdrawnAsync(int applicationId)
        {
            try
            {

                var existingApplication = await _unitOfWork.Application.GetByIdAsync(applicationId);
                if (existingApplication == null)
                {
                    throw new KeyNotFoundException($"Application with ID {applicationId} not found");
                }

                if (existingApplication.Status == "withdrawn")
                {
                    throw new InvalidOperationException($"Application already withdrawn for application ID {applicationId}");
                }
                existingApplication.Status = "withdrawn";

                _unitOfWork.Application.Update(existingApplication);
                await _unitOfWork.SaveChangesAsync();

                var applicationUpdated = await _unitOfWork.Application.ExistsAsync(existingApplication.EmployeeId, existingApplication.JobId);
                return _mapper.Map<ApplicationItemDTO>(applicationUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating apllication with ID: {Id}", applicationId);
                throw;
            }
        }

        public async Task<ApplicationItemDetailDTO> GetApplicationDetailByIdAsync(int applicationId)
        {
            try
            {
                var exist = await _unitOfWork.Application.GetByIdAsync(applicationId);
              

                return _mapper.Map<ApplicationItemDetailDTO>(exist);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving Applications with ID {applicationId}");
                throw;
            }
        }

        public async Task<JobApplicationsResponseDTO> GetJobApplicationsAsync(int employerId, int jobId, int pageNumber, int pageSize)
        {
            try
            {
                var employee = await _unitOfWork.EmployerProfile.ExistsAsync(employerId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employer with ID {employerId} not found");
                }

                var job = await _unitOfWork.Job.GetByIdAsync(jobId);
                if (job == null || job.EmployerId != employerId)
                {
                    throw new KeyNotFoundException($"Job with ID {jobId} not found for employer with ID {employerId}");
                }

                var query = (await _unitOfWork.Application.GetByJobIdAsync(jobId))
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<ApplicationSummaryDTO>>(pagedData);

                return new JobApplicationsResponseDTO
                {
                    EmployerId = employerId,
                    Applications = mapped,
                    JobId=jobId,
                    JobLocation=job.Location,
                    JobTitle=job.Title,
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
                _logger.LogError(ex,$"Error retrieving Applications with JobId: {jobId}");
                throw;
            }
        }

        public async Task<StatsInformationDTOResponse> GetStatsInformationAsync()
        {
            try
            {
                var jobsCount = await _unitOfWork.Job.CountJobsAsync();

                var companiesCount = await _unitOfWork.EmployerProfile.CountCompaniesAsync();

                var candidatesCount = await _unitOfWork.EmployeeProfile.CountCandidatesAsync();

                var newJobsCount = await _unitOfWork.Job.CountNewJobsAsync(TimeSpan.FromDays(7));

                return new StatsInformationDTOResponse
                {
                    Stats = new StatItemDTO
                    {
                        Jobs = jobsCount,
                        Companies = companiesCount,
                        Candidates = candidatesCount,
                        NewJobs = newJobsCount
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stats information");
                throw;
            }
        }

    }
}
