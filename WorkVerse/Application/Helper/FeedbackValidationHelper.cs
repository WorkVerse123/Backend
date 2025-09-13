using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class FeedbackValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidationSendFeedbackRequest(SendFeedbackDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (string.IsNullOrEmpty(request.Content) || request.Content.Length > 255)
                return (false, "Content cannot empty and exceed 255 characters.");

            return (true, string.Empty);
        }
    }
}
