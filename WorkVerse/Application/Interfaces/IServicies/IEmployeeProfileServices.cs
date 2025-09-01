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
        Task<EmployeeProfileDTOResponse> CreateProfileAsync(EmployeeProfileDTOPostRequest employeeProfile);
        Task<EmployeeProfileDTOResponse?> GetByIdAsync(int employeeId);
        Task<EmployeeProfileDTOResponse> UpdateProfileAsync(EmployeeProfileDTOPutRequest employeeProfile);
    }
}
