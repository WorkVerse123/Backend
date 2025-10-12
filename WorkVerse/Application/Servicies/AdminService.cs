using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.IServicies;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminService> _logger;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<StatsResponse> GetSystemStatsAsync()
        {
            try
            {
                return await _unitOfWork.Admin.GetSystemStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminService.GetSystemStatsAsync");
                throw;
            }
        }

        public async Task<ChartResponse> GetChartsAsync(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var userStats = await _unitOfWork.Admin.GetUserChartAsync();
                var jobStats = await _unitOfWork.Admin.GetJobChartAsync(startDate, endDate);
                var paymentStats = await _unitOfWork.Admin.GetPaymentChartAsync(startDate, endDate);

                return new ChartResponse
                {
                    UserStats = userStats,
                    JobStats = jobStats,
                    PaymentStats = paymentStats
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminService.GetChartsAsync");
                throw;
            }
        }

        public async Task<PaginationResult<List<PaymentDTOResponse>>> FilterPayment(PaymentFilterDTORequest filter, int pageNumber, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Admin.FilterPayment(filter, pageNumber, pageSize);

                // Map entity Payment sang DTO
                var paymentDTOs = result.Data.Select(p => new PaymentDTOResponse
                {
                    PaymentId = p.PaymentId,
                    UserId = p.UserId,
                    PlanId = p.PlanId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    Status = p.Status,
                    User = p.User != null ? new UserPaymentRespone { UserId = p.User.UserId } : null,
                    Plan = p.Plan != null ? new PlanPaymentDTORespone
                    {
                        PlanId = p.Plan.PlanId,
                        DurationDays = p.Plan.DurationDays
                    } : null
                }).ToList();

                return new PaginationResult<List<PaymentDTOResponse>>(
                    paymentDTOs,
                    result.TotalRecords,
                    result.PageIndex,
                    result.PageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FilterPayment: Error filtering payments");
                throw;
            }
        }
    }
}
