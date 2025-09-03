using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class ApplicationValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidateApplicationPostRequest(ApplicationDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");
            if (request.JobId == null || request.JobId <= 0)
                return (false, "JobId must be a positive number.");
            if (request.CoverLetter.Length > 255)
                return (false, "Cover Letter cannot exceed 255 characters.");
            return (true, string.Empty);
        }
    }
}
