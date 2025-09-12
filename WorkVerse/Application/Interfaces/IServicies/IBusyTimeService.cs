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
        Task<IEnumerable<BusyTimeItemDTO>> GetBusyTimesByEmployeeAsync(int employeeId);

        Task<IEnumerable<BusyTimeItemDTO>> AddBusyTimesAsync(int employeeId, BusyTimeDTORequest request);

        Task<IEnumerable<BusyTimeItemDTO>> UpdateBusyTimesByEmployeeAsync(int employeeId, BusyTimeDTORequest requests);

        Task<bool> RemoveBusyTimeAsync(int employeeId, int busyTimeId);
    }
}
