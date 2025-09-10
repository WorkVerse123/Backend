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
        Task<JobDTOResponse> GetAllAsync(int pageNumber, int pageSize);
        Task<JobItemDetailDTO> GetByIdAsync(int jobId);
        //Task<JobDTOResponse> GetByIdAsynce(int jobId);
        Task<JobDTOResponse> GetByEmployerIdAsync(int employerId, int pageNumber, int pageSize);
        Task AddAsyne(JobDTORequest job);
        Task<bool> UpdateAsync(int jobId, JobDTORequest job);
        Task<bool> ChangeStatusAsynce(int jobId, string newStatus);

    }
}
