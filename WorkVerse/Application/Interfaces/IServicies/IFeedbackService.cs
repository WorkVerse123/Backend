using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IFeedbackService
    {
        Task<FeedbackItemDTOResponse> CreateFeedbackAsync(SendFeedbackDTORequest request);

        Task<FeedbackListDTOResponse> GetFeedbackListAsync(int pageNumber, int pageSize);

        Task<bool> UpdateFeedbackHandleAsync(int feedbackId, UpdateFeedbackHandlerDTORequest request);
        Task<PaginationResult<List<FeedbackDTOResponse>>> GetAllAsync(int pageIndex = 1, int pageSize = 10);

        Task<FeedbackDTOResponse> UpdateAsync(FeedbackDTORequest entity);

    }
}
