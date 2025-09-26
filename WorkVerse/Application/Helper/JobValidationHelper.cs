using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class JobValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidateJobRequest(JobDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            // Title
            if (string.IsNullOrWhiteSpace(request.Title))
                return (false, "Title is required.");
            if (request.Title.Length > 100)
                return (false, "Title cannot exceed 100 characters.");

            // Description
            if (string.IsNullOrWhiteSpace(request.Description))
                return (false, "Description is required.");
            if (request.Description.Length > 1000)
                return (false, "Description cannot exceed 1000 characters.");

            // Location
            if (string.IsNullOrWhiteSpace(request.Location))
                return (false, "Location is required.");
            if (request.Location.Length > 255)
                return (false, "Location cannot exceed 255 characters.");

            // SalaryMin & SalaryMax
            if (request.SalaryMin < 0)
                return (false, "SalaryMin must be non-negative.");
            if (request.SalaryMax < request.SalaryMin)
                return (false, "SalaryMax must be greater than or equal to SalaryMin.");

            // JobTime
            var allowedTimes = new[] { "FullTime", "PartTime", "Contract", "Internship" };
            if (string.IsNullOrWhiteSpace(request.JobTime))
                return (false, "JobTime is required.");
            if (!allowedTimes.Contains(request.JobTime))
                return (false, $"JobTime must be one of: {string.Join(", ", allowedTimes)}.");

            // ExpiredAt
            if (request.ExpiredAt <= request.CreatedAt)
                return (false, "ExpiredAt must be after CreatedAt.");
            if (request.CreatedAt > DateTime.UtcNow.AddDays(1))
                return (false, "CreatedAt cannot be in the future.");

            // Status
            var allowedStatus = new[] { "Open", "Closed", "Draft" };
            if (string.IsNullOrWhiteSpace(request.Status))
                return (false, "Status is required.");
            if (!allowedStatus.Contains(request.Status))
                return (false, $"Status must be one of: {string.Join(", ", allowedStatus)}.");

            return (true, string.Empty);
        }


		public static string GetDescription<T>(T enumValue) where T : Enum
		{
			var fi = enumValue.GetType().GetField(enumValue.ToString());
			var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
						 .FirstOrDefault() as DescriptionAttribute;
			return attr?.Description ?? enumValue.ToString();
		}

	}
}
