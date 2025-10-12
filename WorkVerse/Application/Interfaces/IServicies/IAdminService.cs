using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IAdminService
    {
        Task<StatsResponse> GetSystemStatsAsync();
        Task<ChartResponse> GetChartsAsync(DateOnly startDate, DateOnly endDate);
        Task<PaginationResult<List<PaymentDTOResponse>>> FilterPayment(PaymentFilterDTORequest filter, int pageNumber, int pageSize);

    }
}
