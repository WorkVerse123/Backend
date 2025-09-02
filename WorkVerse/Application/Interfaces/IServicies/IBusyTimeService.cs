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
    public interface IBusyTimeService
    {
        Task<IEnumerable<BusyTimeDTOResponse>> GetByEmployeeIdAsync(int employeeId);

        Task<IEnumerable<BusyTimeDTOResponse>> CreateBusyTimesAsync(int employeeId, BusyTimeDTORequest request);

    }
}
