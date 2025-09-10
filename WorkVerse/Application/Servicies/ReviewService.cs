using Application.DTOs.Request;
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
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReviewItemDTO> CreateReviewAsync(int jobId, ReviewDTORequest request)
        {
            try
            {

                var employee = await _unitOfWork.EmployeeProfile.ExistsAsync(request.EmployeeId);
                if (!employee)
                {
                    throw new KeyNotFoundException($"Employee with ID {request.EmployeeId} not found");
                }


                var job = await _unitOfWork.Job.ExistsAsync(jobId);
                if (!job)
                {
                    throw new KeyNotFoundException($"Job with ID {jobId} not found");
                }

                var isWorked = await _unitOfWork.Application.ExistsAsync(request.EmployeeId, jobId);
                if (isWorked == null || isWorked.Status != "accepted")
                {
                    throw new InvalidOperationException($"Employee {request.EmployeeId} has never worked on job {jobId}, so evaluation is not allowed.");
                }

                var exists = await _unitOfWork.Review.ExistsAsync(jobId, request.EmployeeId);

                if (exists)
                {
                    throw new InvalidOperationException($"Employee {request.EmployeeId} has already evaluated on job {jobId}");
                }
                var review = new Review
                {
                    EmployeeId = request.EmployeeId,
                    JobId = jobId,
                    Rating = request.Rating,
                    Comment = request.Comment,
                    CreatedAt = DateTime.Now,
                };

                await _unitOfWork.Review.AddAsync(review);
                await _unitOfWork.SaveChangesAsync();

                var reviewAdd = await _unitOfWork.Review.GetByJobIdEmployeeIdAsync(jobId, request.EmployeeId);

                return _mapper.Map<ReviewItemDTO>(reviewAdd);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Error creating review for Employee {request.EmployeeId} and Job {jobId}"  );
                throw;
            }
        }

        public async Task<ReviewDTOResponse> GetByJobIdAsync(int jobId, int pageNumber, int pageSize)
        {
            try
            {
                var job = await _unitOfWork.Job.ExistsAsync(jobId);
                if (!job)
                {
                    throw new KeyNotFoundException($"Job with ID {jobId} not found");
                }

                var query = (await _unitOfWork.Review.GetByJobIdAsync(jobId))
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<ReviewItemDTO>>(pagedData);

                return new ReviewDTOResponse
                {
                    JobId = jobId,
                    Reviews = mapped,
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
                _logger.LogError(ex, "Error retrieving Reviews with JobId: {Id}", jobId);
                throw;
            }
        }
    }
}
