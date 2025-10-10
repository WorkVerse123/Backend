using Application.DTOs.Common;
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
        Task<EmployeeProfileDTOResponse> CreateEmployeeProfileAsync(int userId, EmployeeProfileDTORequest employeeProfile);
        Task<EmployeeProfileDTOResponse?> GetEmployeeProfileByIdAsync(int employeeId);
        Task<EmployeeProfileDTOResponse> UpdateEmployeeProfileAsync(int employeeId, EmployeeProfileDTORequest employeeProfile);
        Task<EmployeeProfileDTOResponse?> GetEmployeeProfileByUserIdAsync(int userId);
        Task<CandidateListDTOResponse> GetEmployeeListAsync(int pageNumber, int pageSize);
        Task<EmployeeDashboardDTOResponse> GetEmployeeDashBoardAsync(int employeeId ,int pageNumber, int pageSize);
        Task<int?> GetEmployeeIdByUserIdAsync(int userId);
        Task<IEnumerable<EmployeeAIDTOResponse>> SearchEmployeeByAIResult(EmployeeQuery employeeQuery);
        Task<bool> UpdatePriority(int employeeId, bool isPriority);
        Task<PaginationResult<List<EmployeeProfileDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10);

        Task<EmployeeProfileDTOResponse> UpdateAsync(EmployeeProfileUpdateDTORequest entity);


    }
}
