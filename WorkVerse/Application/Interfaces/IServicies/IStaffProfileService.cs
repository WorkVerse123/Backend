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
    public interface IStaffProfileService
    {
        Task<StaffProfileDTOResponse> Get(int id);
        Task<StaffProfileDTOResponse> Create(StaffProfileDTORequest request);
        Task<StaffProfileDTOResponse> Update(StaffProfileDTORequest staffProfile);
        Task<PaginationResult<List<StaffProfileDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10);

        Task<StaffProfileDTOResponse> UpdateAsync(StaffProfileDTORequest entity);
    }
}
