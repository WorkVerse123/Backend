using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IBusyTimeRepository : IGenericRepository<BusyTime>
    {
        Task<IEnumerable<BusyTime>> GetByEmployeeIdAsync(int employeeId);

        Task<bool> ExistsOverlapAsync(int employeeId, byte dayOfWeek, TimeSpan start, TimeSpan end, int? excludeBusyTimeId);

        Task<BusyTime> GetByIdAsync(int busyTimeId);
    }

}
