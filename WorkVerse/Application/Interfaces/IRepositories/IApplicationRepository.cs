using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IApplicationRepository: IGenericRepository<Domain.Entities.Application>
    {
        Task<IEnumerable<Domain.Entities.Application>> GetByEmployeeIdAsync(int employeeId);
        Task<Domain.Entities.Application> GetByIdAsync(int applicationId);
    }
}
