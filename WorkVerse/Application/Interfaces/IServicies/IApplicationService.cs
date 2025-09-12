using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IApplicationService
    {
        Task<JobApplicationListDTOResponse> GetApplicationsByEmployeeAsync(int employeeId, int pageNumber, int pageSize);
        Task<JobApplicationItemDTO> CreateApplyJobAsync(int employeeId, ApplicationDTORequest request);
        Task<JobApplicationItemDTO> WithdrawApplicationAsync(int applicationId);
        Task<JobApplicationDetailsDTOResponse> GetApplicationDetailsByIdAsync(int applicationId);
        Task<EmployerJobApplicationsDTOResponse> GetApplicationsByJobAsync(int employerId, int jobId,int pageNumber, int pageSize);

        Task<PlatformStatsResponseDTOResponse> GetPlatformStatsAsync();
    }
}
