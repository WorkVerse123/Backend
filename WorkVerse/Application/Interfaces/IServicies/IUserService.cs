using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IUserService
    {
        Task<bool> UpdateStatusAsync(int userId, string newStatus);
        Task<PaginationResult<List<UserDTORespone>>> GetAllAsync(int pageIndex = 1, int pageSize = 10);

        Task<UserDTORespone> UpdateAsync(UserUpdateDTORequest entity);
    }
}
