using Application.DTO.Request;
using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class EmployeeProfileValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidateEmployeeProfileRequest(EmployeeProfileDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");
            // FullName
            if (string.IsNullOrWhiteSpace(request.FullName))
                return (false, "FullName is required.");
            if (request.FullName.Length > 100)
                return (false, "FullName cannot exceed 100 characters.");
            // DateOfBirth
            if (request.DateOfBirth.HasValue)
            {
                var dob = request.DateOfBirth.Value;
                if (dob > DateTime.Now)
                    return (false, "DateOfBirth cannot be in the future.");

                var today = DateTime.Now.Date;
                int age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--; 

                if (age < 15)
                    return (false, "Employee must be at least 15 years old.");
            }
            // Gender
            var allowedGenders = new[] { "Male", "Female", "Other" };
            if (!string.IsNullOrWhiteSpace(request.Gender) && !allowedGenders.Contains(request.Gender))
                return (false, $"Gender must be one of: {string.Join(", ", allowedGenders)}.");
            // Mode
            var allowedModes = new[] { "private", "public" };
            if (!string.IsNullOrWhiteSpace(request.Mode) && !allowedModes.Contains(request.Mode))
                return (false, $"Mode must be one of: {string.Join(", ", allowedModes)}.");
            return (true, string.Empty);
        }

    }
}
