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
    public interface IBookmarkService
    {
        Task<JobBookmarkListDTOResponse> GetBookmarksByEmployeeAsync(int employeeId, int pageNumber, int pageSize);

        Task<JobBookmarkItemDTO> AddBookmarkAsync(int employeeId, int jobId);
        Task<bool> RemoveBookmarkAsync(int employeeId, int bookmarkId);

    }
}
