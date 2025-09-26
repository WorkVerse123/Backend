using Application.DTOs.Request;
using Domain.Entities;
using System.Text.RegularExpressions;


public static class UserValidationHelper
{
    public static (bool IsValid, string ErrorMessage) ValidateLogin(UserLoginDTORequest request)
    {
        if (request == null)
            return (false, "Request is null.");
        if (string.IsNullOrWhiteSpace(request.Email))
            return (false, "Email is required.");

        if (!string.IsNullOrWhiteSpace(request.Email) && !request.Email.Contains("@"))
            return (false, "Email is not valid.");
        //if (request.Password.Length < 6)
        //    return (false, "Password must be at least 6 characters.");
        return (true, string.Empty);
    }

    public static (bool IsValid, string ErrorMessage) ValidateRegister(UserDTORequest request)
    {
        if (request == null)
            return (false, "Request is null.");

        // Email
        if (string.IsNullOrWhiteSpace(request.Email))
            return (false, "Email is required.");
        if (!request.Email.Contains("@"))
            return (false, "Email is not valid.");

        // Phone
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return (false, "Phone number is required.");
        if (request.PhoneNumber.Length != 10)
            return (false, "Phone number must be 10 digits.");
        if (!request.PhoneNumber.All(char.IsDigit))
            return (false, "Phone number must contain only digits.");

        // Password
        if (string.IsNullOrWhiteSpace(request.Password))
            return (false, "Password is required.");
        //if (request.Password.Length < 6)
        //    return (false, "Password must be at least 6 characters.");

        // Status
        if (string.IsNullOrWhiteSpace(request.Status))
            return (false, "Status is required.");

        if (request.Status.ToLower() != "active")
            return (false, "Status must be 'active'.");

        return (true, string.Empty);
    }
    public static (bool IsValid, string ErrorMessage) ValidateChangePassword(UserChangePasswordDTORequest request)
    {
        if (request == null)
            return (false, "Request is null.");
        // Current Password
        if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            return (false, "Current password is required.");
        //if (request.CurrentPassword.Length < 6)
        //    return (false, "Current password must be at least 6 characters.");
        // New Password
        if (string.IsNullOrWhiteSpace(request.NewPassword))
            return (false, "New password is required.");
        //if (request.NewPassword.Length < 6)
        //    return (false, "New password must be at least 6 characters.");
        if (request.NewPassword == request.CurrentPassword)
            return (false, "New password must be different from current password.");
        return (true, string.Empty);
    }
}
