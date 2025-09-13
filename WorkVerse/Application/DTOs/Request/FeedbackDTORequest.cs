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
}
