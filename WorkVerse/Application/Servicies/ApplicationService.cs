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

                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }


                var job = await _unitOfWork.Job.GetByIdAsync(request.JobId);
                if (job == null)
                {
                    throw new KeyNotFoundException($"Job with ID {request.JobId} not found");
                }

                var exists = await _unitOfWork.Application.ExistsAsync(employeeId, request.JobId);
                if (exists != null)
                {
                    throw new InvalidOperationException($"Application already exists for Employee {employeeId} and Job {request.JobId}");
                }


                var application = new Domain.Entities.Application
                {
                    EmployeeId = employeeId,
                    JobId = request.JobId,
                    AppliedAt = DateTime.UtcNow,
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

        public async Task<ApplicationResponseDTO> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }

                var entities = await _unitOfWork.Application.GetByEmployeeIdAsync(employeeId);

                if (entities == null || !entities.Any())
                {
                    return new ApplicationResponseDTO
                    {
                        Applications = new List<ApplicationItemDTO>()
                    };
                }

                var mapped = _mapper.Map<List<ApplicationItemDTO>>(entities);

                return new ApplicationResponseDTO
                {
                    Applications = mapped
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Applications with EmployeeId: {Id}", employeeId);
                throw;
            }
        }
    }
}
