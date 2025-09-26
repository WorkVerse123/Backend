using Application.DTOs.Request;
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
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<IEnumerable<Job>> GetJobsByEmployerIdAsync(int employerId);

        Task<bool> ExistsByJobIdAsync(int jobId);

        Task<int> CountAllJobsAsync();

        Task<int> CountNewJobsAsync(TimeSpan range);

        Task<string?> GetJobTitleByIdAsync(int id);

        Task<IEnumerable<Job>> SearchJobByAIResult(JobQuery jobQuery);

        Task<IEnumerable<Job>> SearchJobByEmployerAIResult(JobQuery jobQuery, EmployerQuery employerQuery);
    }
}
