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
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployerProfileService> _logger;

        public EmployerProfileService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployerProfileService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployerProfileDTOResponse> GetAllCompaniesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.EmployerProfile.GetAllCompaniesAsync())
                            .AsQueryable();

                var totalRecords = query.Count();

                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var mapped = _mapper.Map<List<CompanyItemDTO>>(pagedData);

                return new EmployerProfileDTOResponse
                {
                    Companies = mapped,
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
                _logger.LogError(ex, "Error retrieving Companies");
                throw;
            }
        }
    }
}
