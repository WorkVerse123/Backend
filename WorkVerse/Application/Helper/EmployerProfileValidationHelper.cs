using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class EmployerProfileValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidateEmployerProfilePostRequest(EmployerProfileDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            // CompanyName
            if (string.IsNullOrWhiteSpace(request.CompanyName))
                return (false, "CompanyName is required.");
            if (request.CompanyName.Length > 255)
                return (false, "CompanyName cannot exceed 255 characters.");

            // EmployerType
            if (request.EmployerTypeId <= 0)
                return (false, "EmployerType must be a valid positive number.");

            // Address
            if (string.IsNullOrWhiteSpace(request.Address))
                return (false, "Address is required.");
            if (request.Address.Length > 255)
                return (false, "Address cannot exceed 255 characters.");

            // Description
            if (string.IsNullOrWhiteSpace(request.Description))
                return (false, "Description is required.");

            // DateEstablished
            if (request.DateEstablish.HasValue && request.DateEstablish.Value > DateTime.Now)
                return (false, "DateEstablished cannot be in the future.");

            // WebsiteUrl
            if (!string.IsNullOrWhiteSpace(request.WebsiteUrl) &&
                !Uri.TryCreate(request.WebsiteUrl, UriKind.Absolute, out _))
            {
                return (false, "WebsiteUrl must be a valid URL.");
            }

            // LogoUrl
            if (!string.IsNullOrWhiteSpace(request.LogoUrl) &&
                !Uri.TryCreate(request.LogoUrl, UriKind.Absolute, out _))
            {
                return (false, "LogoUrl must be a valid URL.");
            }
            // Phone
            if (string.IsNullOrWhiteSpace(request.ContactPhone))
                return (false, "Phone number is required.");
            if (request.ContactPhone.Length != 10)
                return (false, "Phone number must be 10 digits.");
            if (!request.ContactPhone.All(char.IsDigit))
                return (false, "Phone number must contain only digits.");

            // Email
            if (string.IsNullOrWhiteSpace(request.ContactEmail))
                return (false, "Email is required.");
            if (!request.ContactEmail.Contains("@"))
                return (false, "Email is not valid.");

            return (true, string.Empty);
        }

    }
}
