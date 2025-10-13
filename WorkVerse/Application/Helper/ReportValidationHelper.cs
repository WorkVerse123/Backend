
using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class ReportValidationHelper
    {
        private static readonly HashSet<string> AllowedTargetTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "job", "user"
    };

        public static (bool IsValid, string ErrorMessage) ValidateSubmitReport(SubmitReportDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.TargetType))
                return (false, "TargetType is required.");

            if (!AllowedTargetTypes.Contains(request.TargetType))
                return (false, $"TargetType must be one of: {string.Join(", ", AllowedTargetTypes)}");


            if (string.IsNullOrWhiteSpace(request.Reason))
                return (false, "Reason is required.");

            if (request.Reason.Length > 500)
                return (false, "Reason must not exceed 500 characters.");

            return (true, string.Empty);
        }
    }

}
