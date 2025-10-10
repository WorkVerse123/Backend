using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.IServicies;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}
