using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.ExceptionHandler;
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
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReportDetailsDTOResponse> CreateReportAsync(SubmitReportDTORequest request)
        {
            try
            {

                var user = await _unitOfWork.User.ExistByIdAsync(request.UserId);
                if (!user)
                {
                    throw new InvalidOperationException($"User with ID {request.UserId} not found");
                }

                request.TargetType = request.TargetType.ToLower();
                switch (request.TargetType.ToLower())
                {
                    case "job":
                        if (!await _unitOfWork.Job.ExistsByJobIdAsync(request.TargetId))
                            throw new InvalidOperationException($"Job with Target ID {request.TargetId} not found");
                        break;

                    case "user":
                        if (!await _unitOfWork.User.ExistByIdAsync(request.TargetId))
                            throw new InvalidOperationException($"User with Target ID {request.TargetId} not found");
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported target type: {request.TargetType}");
                }

                var exists = await _unitOfWork.Report.ExistsByUserAndTargetAsync(
                    request.UserId,
                    request.TargetType,
                    request.TargetId
                );

                if (exists)
                {
                    throw new InvalidOperationException(
                        $"User {request.UserId} has already submitted a report for {request.TargetType} with ID {request.TargetId}"
                    );
                }
                var report = _mapper.Map<Report>(request);

                await _unitOfWork.Report.AddAsync(report);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<ReportDetailsDTOResponse>(report);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
    "Error creating report for User {UserId}, TargetType {TargetType}, TargetId {TargetId}",
    request.UserId, request.TargetType, request.TargetId);
                throw;
            }
        }

        public async Task<ReportListDTOResponse> GetReportListAsync(int pageNumber, int pageSize)
        {

            var query = (await _unitOfWork.Report.GetReportsAsync())
                            .AsQueryable().OrderByDescending(c => c.ReportedAt);

            var totalRecords = query.Count();

            var pagedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new List<ReportIncludeReporterDTOResponse>();

            foreach (var report in pagedData)
            {
                string title = string.Empty;

                switch (report.TargetType.ToLower())
                {
                    case "job":
                        title = await _unitOfWork.Job.GetJobTitleByIdAsync(report.TargetId)
                        ?? "(Job not found)";
                        break;

                    case "user":
                        title = await _unitOfWork.User.GetUserFullNameByIdAsync(report.TargetId)
                       ?? "(User not found)";
                        break;


                }

                result.Add(new ReportIncludeReporterDTOResponse
                {
                    ReportId = report.ReportId,
                    Reason = report.Reason,
                    Status = report.Status,
                    ReportedAt = report.ReportedAt,
                    Reporter = new ReporterDTOResponse
                    {
                        UserId = report.User.UserId,
                        FullName = await _unitOfWork.User.GetUserFullNameByIdAsync(report.User.UserId)
                    },
                    Target = new ReportTargetDTOResponse
                    {
                        Type = report.TargetType,
                        Id = report.TargetId,
                        Title = title
                    }
                });

            }

            return new ReportListDTOResponse
            {
                Paging = new PaginatedResponse
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                },
                Reports = result
            };
        }

        public async Task<bool> UpdateReportStatusAsync(int reportId, UpdateReportStatusDTORequest request)
        {
            try
            {
                var report = await _unitOfWork.Report.GetReportByIdAsync(reportId);
                if (report == null)
                {
                    throw new KeyNotFoundException($"Report with ID {reportId} not found.");
                }
                if (request.Status.ToLower() != "pending" && request.Status.ToLower() != "resolved" && request.Status.ToLower() != "dismissed")
                {
                    throw new InvalidOperationException($"Status '{request.Status}' is not valid. It must be pending, resolved or dismissed");
                }
                report.Status = request.Status;
                _unitOfWork.Report.Update(report);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status of Report with ID: {Id}", reportId);
                throw;
            }

        }
    }
}
