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
    public class BusyTimeService : IBusyTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BusyTimeService> _logger;

        public BusyTimeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BusyTimeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BusyTimeDTOResponse>> GetByEmployeeIdAsync(int employeeId)
        {
            try
            {
                var entities = await _unitOfWork.BusyTime.GetByEmployeeIdAsync(employeeId);

                if (entities == null || !entities.Any())
                {
                    return Enumerable.Empty<BusyTimeDTOResponse>();
                }

                return _mapper.Map<IEnumerable<BusyTimeDTOResponse>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving BusyTime with EmployeeId: {Id}", employeeId);
                throw;
            }
        }

    }
}
