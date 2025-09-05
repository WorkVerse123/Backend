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

        public async Task<BookmarkDTOResponse> GetByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(employeeId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }

                var query = (await _unitOfWork.Bookmark.GetByEmployeeIdAsync(employeeId))
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<BookmarkItemDTO>>(pagedData);

                return new BookmarkDTOResponse
                {
                    EmployeeId = employeeId,
                    Bookmarks = mapped,
                    Paging = new PaginatedResponse
                    {
                        Page = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Bookmarks with EmployeeId: {Id}", employeeId);
                throw;
            }
        }



        public async Task<BookmarkItemDTO> CreateBookmarkAsync(int employeeId, int jobId)
        {
            try
            {

                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(employeeId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
                }


                var job = await _unitOfWork.Job.ExistsAsync(jobId);
                if (!job)
                {
                    throw new KeyNotFoundException($"Job with ID {jobId} not found");
                }

                var exists = await _unitOfWork.Bookmark.ExistsAsync(employeeId, jobId);
                if (exists != null)
                {
                    throw new InvalidOperationException($"Bookmark already exists for Employee {employeeId} and Job {jobId}");
                }


                var bookmark = new Bookmark
                {
                    EmployeeId = employeeId,
                    JobId = jobId,
                    SavedAt = DateTime.Now
                };

                await _unitOfWork.Bookmark.AddAsync(bookmark);
                await _unitOfWork.SaveChangesAsync();

                exists = await _unitOfWork.Bookmark.ExistsAsync(employeeId, jobId);
                return _mapper.Map<BookmarkItemDTO>(exists);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bookmark for Employee {EmployeeId} and Job {JobId}", employeeId, jobId);
                throw;
            }
        }

        public async Task<bool> DeleteBookmarkAsync(int employeeId, int bookmarkId)
        {
            try
            {

                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(employeeId);
                if (!employee)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");


                var bookmark = await _unitOfWork.Bookmark.GetByIdAsync(bookmarkId);
                if (bookmark == null || bookmark.EmployeeId != employeeId)
                    throw new KeyNotFoundException(
                        $"BusyTime with ID {bookmarkId} not found for Employee {employeeId}");


                _unitOfWork.Bookmark.HardRemove(bookmark);


                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bookmark {BookmarkId} for employee {EmployeeId}", bookmarkId, employeeId);
                throw;
            }
        }

    }
}
