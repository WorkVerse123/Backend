using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class SendFeedbackDTORequest
    {
        public int UserId { get; set; }
        public string Content { get; set; } = null!;

    }
    public class UpdateFeedbackHandlerDTORequest
    {
        public int HandleBy { get; set; }

    }
    public class FeedbackDTORequest
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int? HandledBy { get; set; }
        public DateTime? HandledAt { get; set; }
    }
}
