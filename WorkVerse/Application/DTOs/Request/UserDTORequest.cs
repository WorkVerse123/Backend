using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class UserDTORequest
    {
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string Status { get; set; } = "Active";

    }
    public class UserLoginDTORequest
    {
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserChangePasswordDTORequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
    public class UserForgotPasswordDTORequest
    {
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

    }
    public class UserLogoutDTORequest
    {
        public int UserId { get; set; }
    }
}
