using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class EmployerProfileDTORequest
    {
        public int EmployerId { get; set; } // 
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!; //
        public int EmployerType { get; set; }
        public string Address { get; set; } = null!; //
        public string? WebsiteUrl { get; set; } //
        public string? LogoUrl { get; set; } //
        public DateTime? DateEstablished { get; set; } //
        public string Description { get; set; } = null!; //
        public string ContactNumber { get; set; } //
        public string ContactEmail { get; set; } //

    }
}
