using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookmarkService> _logger;

        public BookmarkService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookmarkService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookmarkDTOResponse> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeProfile.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }

                var entities = await _unitOfWork.Bookmark.GetByEmployeeIdAsync(employeeId);

                if (entities == null || !entities.Any())
                {
                    return new BookmarkDTOResponse
                    {
                        EmployeeId = employeeId,
                        Bookmarks = new List<BookmarkItemDTO>()
                    };
                }

                var mapped = _mapper.Map<List<BookmarkItemDTO>>(entities);

                return new BookmarkDTOResponse
                {
                    EmployeeId = employeeId,
                    Bookmarks = mapped
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Bookmarks with EmployeeId: {Id}", employeeId);
                throw;
            }
        }

    }
}
