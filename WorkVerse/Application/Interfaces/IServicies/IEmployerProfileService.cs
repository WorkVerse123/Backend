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
    public interface IEmployerProfileService
    {
        Task<ListEmployerProfileDTOResponse> GetAllEmployersAsync(int pageNumber, int pageSize);
        Task<bool> CreateEmployerProfileAsync(EmployerProfileDTORequest request);
        Task<EmployerProfileDTOResponse> GetEmployerProfileByIdAsync(int id);
        Task<bool> UpdateEmployerProfileAsync(int id, EmployerProfileDTORequest request);
        Task<int?> GetEmployerIdByUserIdAsync(int userId);
        Task<IEnumerable<EmployerAIDTOResponse>> SearchEmployerByAIResult(EmployerQuery employerQuery);
        Task<bool> UpdateUserByEmployerId(int employerId, string newPhone, string newEmail);
        Task<ListEmployerProfileFilterDTOResponse> GetEmployersFilter(EmployerFilterRequest filter, int pageNumber, int pageSize);
    }
}
