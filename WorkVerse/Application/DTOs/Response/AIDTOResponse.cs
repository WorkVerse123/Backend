using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{

    public class JobAIDTOResponse
    {
        public int JobId { get; set; }
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
        public bool IsPriority { get; set; }
        public List<string> Categories { get; set; } = new();
        public List<ShiftDTO> Shifts { get; set; } = new();
    }
    public class ShiftDTO
    {
        public List<byte> DaysOfWeek { get; set; } = new(); // 0=Sunday, 1=Monday...
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
    public class JobWithEmployerAIDTOResponse
    {
        public int JobId { get; set; }
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string JobTime { get; set; } = null!;

        public List<string> Categories { get; set; } = new();

        public List<ShiftDTO> Shifts { get; set; } = new();

        // Thông tin Employer
        public int EmployerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string EmployerType { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public bool IsPriority { get; set; } = false;

    }

    public class EmployerAIDTOResponse
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string EmployerType { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? WebsiteUrl { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime? DateEstablish { get; set; }
        public string Description { get; set; } = null!;
        public bool IsPriority { get; set; } = false;

    }

    public class EmployeeAIDTOResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public bool IsPriority { get; set; } = false;

    }

}
