using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IReportService
    {
        Task<ReportDetailsDTOResponse> CreateReportAsync(SubmitReportDTORequest request);

        Task<ReportListDTOResponse> GetReportListAsync(int pageNumber, int pageSize);

        Task<bool> UpdateReportStatusAsync(int reportId, UpdateReportStatusDTORequest request);
        Task<PaginationResult<List<ReportDTORespone>>> GetAllAsync(int pageIndex = 1, int pageSize = 10);

        Task<ReportDTORespone> UpdateAsync(ReportDTORequest entity);

    }
}
