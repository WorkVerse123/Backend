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
    public interface IJobService
    {
        Task<JobListDTOResponse> GetJobListAsync(int pageNumber, int pageSize);
        Task<JobDetailsDTOResponse> GetJobByIdAsync(int jobId);
        //Task<JobDTOResponse> GetByIdAsynce(int jobId);
        Task<JobListDTOResponse> GetJobsByEmployerIdAsync(int employerId, int pageNumber, int pageSize);
        Task AddJobAsync(JobDTORequest job);
        Task<bool> UpdateJobAsync(int jobId, JobDTORequest job);
        Task<bool> UpdateJobStatusAsync(int jobId, string newStatus);

        Task<IEnumerable<JobAIDTOResponse>> SearchJobByAIResult(JobQuery jobQuery);
        Task<IEnumerable<JobWithEmployerAIDTOResponse>> SearchJobByEmployerAIResult(JobQuery jobQuery, EmployerQuery employerQuery);

    }
}
