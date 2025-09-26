using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class UserDTORespone
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Token { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsPremium { get; set; }
        public int EmployeeId { get; set; }
        public int EmployerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
