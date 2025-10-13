using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class BlogValidationHelper
    {
        public static (bool IsValid, string ErrorMessage) ValidationCreateBlogRequest(CreateBlogDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 150)
                return (false, "Title is required and cannot exceed 150 characters.");

            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
                return (false, "Content is required and cannot exceed 5000 characters.");

            if (!string.IsNullOrWhiteSpace(request.ImageUrl) &&
                !Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out _))
                return (false, "ImageUrl must be a valid URL.");

            var allowedStatuses = new[] { "draft", "published" };
            if (string.IsNullOrWhiteSpace(request.Status) || !allowedStatuses.Contains(request.Status.ToLower()))
                return (false, $"Status must be one of: {string.Join(", ", allowedStatuses)}");

            return (true, string.Empty);
        }

        public static (bool IsValid, string ErrorMessage) ValidationUpdateBlogRequest(UpdateBlogDTORequest request)
        {
            if (request == null)
                return (false, "Request cannot be null.");

            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 150)
                return (false, "Title is required and cannot exceed 150 characters.");

            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 5000)
                return (false, "Content is required and cannot exceed 5000 characters.");

            if (!string.IsNullOrWhiteSpace(request.ImageUrl) &&
                !Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out _))
                return (false, "ImageUrl must be a valid URL.");

            var allowedStatuses = new[] { "draft", "published" };
            if (string.IsNullOrWhiteSpace(request.Status) || !allowedStatuses.Contains(request.Status.ToLower()))
                return (false, $"Status must be one of: {string.Join(", ", allowedStatuses)}");

            return (true, string.Empty);
        }
    }
}
