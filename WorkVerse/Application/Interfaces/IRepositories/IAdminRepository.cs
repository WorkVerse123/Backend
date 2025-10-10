using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IAdminRepository
    {
        public Task<StatsResponse> GetSystemStatsAsync();
        Task<ChartUserResponse> GetUserChartAsync();
        Task<List<ChartJobResponse>> GetJobChartAsync(DateOnly startDate, DateOnly endDate);
        Task<List<ChartPaymentResponse>> GetPaymentChartAsync(DateOnly startDate, DateOnly endDate);
    }
}
