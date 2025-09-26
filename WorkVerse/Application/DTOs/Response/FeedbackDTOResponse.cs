using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class FeedbackItemDTOResponse
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int? HandledBy { get; set; }
        public DateTime? HandledAt { get; set; }

    }

    public class FeedbackListDTOResponse
    {
        public PaginatedResponse Paging { get; set; } = null!;
        public List<FeedbackItemDetailsDTOResponse> Feedbacks { get; set; } = new();
    }

    public class FeedbackItemDetailsDTOResponse
    {
        public int FeedbackId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;

        public FeedbackUserDTOResponse User { get; set; } = null!;
        public FeedbackHandlerDTOResponse? Handler { get; set; }
        public DateTime? HandledAt { get; set; }
    }

    public class FeedbackUserDTOResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
    }

    public class FeedbackHandlerDTOResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
    }

}
