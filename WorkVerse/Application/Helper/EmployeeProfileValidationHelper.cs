using Application.DTOs.Request;
using Domain.Entities;
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

    public static class EducationMatchingHelper
    {
        private static readonly Dictionary<EmployeeEducation, string[]> _educationKeywords = new()
    {
        // Trung học phổ thông
    { EmployeeEducation.HighSchool, new[]
        {
            "thpt", "trung học", "trung hoc", "phổ thông", "pho thong", "12/12",
            "tốt nghiệp 12", "tot nghiep 12", "hết cấp 3", "cap 3", "c3", "trường cấp 3",
            "đã học thpt", "đang học thpt"
        }
    },

    // Cao đẳng
    { EmployeeEducation.College, new[]
        {
            "cao đẳng", "cao dang", "cd", "tốt nghiệp cao đẳng", "hoc cao đẳng", "học cd",
            "đã học cd", "sinh viên cao đẳng", "đang học cao đẳng"
        }
    },

    // Đại học
    { EmployeeEducation.University, new[]
        {
            "đại học", "dai hoc", "đh", "dh", "sinh viên", "tot nghiep dai hoc",
            "tốt nghiệp đại học", "hoc dai hoc", "đã học đại học", "cử nhân",
            "học đại học", "đang học đh", "đã tốt nghiệp đh", "sinh vien dh"
        }
    },

    // Sau đại học
    { EmployeeEducation.Postgraduate, new[]
        {
            "sau đại học", "sau dai hoc", "thạc sĩ", "thac si", "tiến sĩ", "tien si",
            "cao học", "cao hoc", "thuc tap sinh cao hoc", "đã học thạc sĩ", "đang học cao học"
        }
    },
    { EmployeeEducation.None, new[]
    {
        "lao động phổ thông", "lao dong pho thong", "thất nghiệp", "that nghiep",
        "tự do", "tu do", "không đi học", "chưa học", "không bằng", "không có bằng cấp",
        "không học vấn", "không học", "chưa tốt nghiệp", "bỏ học"
    } }
    };

        public static bool MatchesEducation(EmployeeProfile employee, List<int> filterEducationIds)
        {
            if (employee.Education == null || filterEducationIds == null || !filterEducationIds.Any())
                return true; // không lọc nếu không có filter hoặc dữ liệu trống

            var educationText = employee.Education.ToLower();

            foreach (var eduId in filterEducationIds)
            {
                if (_educationKeywords.TryGetValue((EmployeeEducation)eduId, out var keywords))
                {
                    if (keywords.Any(k => educationText.Contains(k)))
                        return true;
                }
            }

            return false;
        }
    }

}
