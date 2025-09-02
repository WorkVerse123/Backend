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
        Task<BookmarkDTOResponse> GetByEmployeeIdAsync(int employeeId);

        Task<BookmarkItemDTO> CreateBookmarkAsync(int employeeId, int jobId);
        Task<bool> DeleteBookmarkAsync(int employeeId, int bookmarkId);

    }
}
