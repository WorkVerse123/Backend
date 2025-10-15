
using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<UserDTORespone?> ValidateUserAsync(string account, string password);
        Task<UserDTORespone?> CreatedAccountAsync(UserDTORequest user);
        Task<bool> ExsitedUser(string? email);
        Task<bool> ExsitedRole(int roleId);
        Task<bool> UpdatePasswordAsync(UserChangePasswordDTORequest user);
        Task<bool> IsPremiumAsync(int userId);
        bool VerifyPassword (string password, string hashPassword);
        string EncryptPassword(string password);
    }
}
