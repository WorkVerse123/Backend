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
        Task<Domain.Entities.Application> ExistsAsync(int employeeId, int jobId);
        Task<bool> ExistsAsync(int applicationId);

        Task<IEnumerable<Domain.Entities.Application>> GetByJobIdAsync(int jobId);


    }
}
