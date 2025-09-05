using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class JobCategoryService : IJobCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<JobCategoryService> _logger;

        public JobCategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<JobCategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<JobCategoryDTOResponse> GetAllJobCategoriesAsync()
        {
            try
            {
                var jobCategories = await _unitOfWork.JobCategory.GetAllAsync();

                var mapped = _mapper.Map<List<JobCategoryItemDTO?>>(jobCategories);

                return new JobCategoryDTOResponse
                {
                    JobCategories = mapped
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving JobCategories");
                throw;
            }
        }
    }
}
