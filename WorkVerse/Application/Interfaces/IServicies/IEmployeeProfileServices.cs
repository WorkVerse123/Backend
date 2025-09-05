using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IEmployeeProfileServices
    {
        Task<EmployeeProfileDTOResponse> CreateProfileAsync(int userId, EmployeeProfileDTORequest employeeProfile);
        Task<EmployeeProfileDTOResponse?> GetByIdAsync(int employeeId);
        Task<EmployeeProfileDTOResponse> UpdateProfileAsync(int employeeId, EmployeeProfileDTORequest employeeProfile);
        Task<EmployeeProfileDTOResponse?> GetByUserIdAsync(int userId);
        Task<CandidateDTOResponse> GetAllCandidatesAsync(int pageNumber, int pageSize);

    }
}
