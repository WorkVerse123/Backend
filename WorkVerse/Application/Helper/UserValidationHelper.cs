using Application.DTO.Request;
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
        if (string.IsNullOrWhiteSpace(request.Password))
            return (false, "Password is required.");
        //if (request.Password.Length < 6)
        //    return (false, "Password must be at least 6 characters.");
        return (true, string.Empty);
    }

   
}
