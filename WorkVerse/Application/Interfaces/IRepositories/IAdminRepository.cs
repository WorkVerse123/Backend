using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;
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
        Task<PaginationResult<List<Payment>>> FilterPayment(PaymentFilterDTORequest filter, int pageNumber, int pageSize);
    }
}
