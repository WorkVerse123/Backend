using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class StaffProfileDTOResponse
    {
        public int StaffId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? IdentityCard { get; set; }   // CCCD
        public DateTime StartDate { get; set; }
    }
}
