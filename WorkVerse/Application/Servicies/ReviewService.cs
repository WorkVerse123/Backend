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
