using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class ReviewValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidateReviewPostRequest(ReviewDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (request.EmployeeId <= 0)
                return (false, "EmployeeId must be a positive number.");

            if (request.Rating < 1 || request.Rating > 5)
                return (false, "Rating must be between 1 and 5.");

            if (!string.IsNullOrEmpty(request.Comment) && request.Comment.Length > 255)
                return (false, "Comment cannot exceed 255 characters.");

            return (true, string.Empty);
        }
    }
}
