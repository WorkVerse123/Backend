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
        Task<ApplicationResponseDTO> GetByEmployeeIdAsync(int employeeId);
        Task<ApplicationItemDTO> CreateApplicationAsync(int employeeId, ApplicationDTORequest request);
        Task<ApplicationItemDTO> UpdateApplicationWithdrawnAsync(int applicationId);
    }
}
