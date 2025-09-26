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
    public class EmployerTypeService : IEmployerTypeSevice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployerTypeService> _logger;
        public EmployerTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployerTypeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<EmployerTypeDTOResponse>> GetAllEmployerTypesAsync()
        {
            try
            {
                var entities = await _unitOfWork.EmployerType.GetAllEmployerTypesAsync();
                var mapped = _mapper.Map<List<EmployerTypeDTOResponse>>(entities);
                return mapped;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employer types");
                throw;
            }
        }
    }
}
