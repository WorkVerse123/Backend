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
    public interface IReviewService
    {
        Task<JobReviewListDTOResponse> GetJobReviewsByJobIdAsync(int jobId, int pageNumber, int pageSize);

        Task<JobReviewItemDTO> CreateJobReviewAsync(int jobId, ReviewDTORequest request);

    }
}
