using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<Job> GetByIdAsync(int jobId);
        Task<IEnumerable<Job>> GetAllAsync();

        Task<bool> ExistsAsync(int jobId);

        Task<int> CountJobsAsync();

        Task<int> CountNewJobsAsync(TimeSpan range);
    }
}
