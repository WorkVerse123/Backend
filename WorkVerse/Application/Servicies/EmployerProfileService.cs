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

        public async Task<bool> CreateEmployerProfileAsync(EmployerProfileDTORequest request)
        {
            try
            {
               
                var existingProfile = await _unitOfWork.EmployerProfile.CheckExistByUserIdAsync(request.UserId);
                if (existingProfile)
                {
                    throw new InvalidOperationException($"Employer profile already exists for User {request.UserId}");
                }

                var entity = _mapper.Map<EmployerProfile>(request);

                await _unitOfWork.EmployerProfile.AddAsync(entity);
                var result = await _unitOfWork.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employer profile for UserId: {UserId}", request.UserId);
                throw;
            }
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
