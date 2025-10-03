using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // hồ sơ cá nhân của ứng viên   
    public class EmployeeProfileDTOResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public string? Mode { get; set; }
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }

    public class EmployeeDashboardDTOResponse
    {
        public List<DashboardStat> Stats { get; set; } = new();
        public PaginatedResponse Paging { get; set; }
        public List<EmployeeApplicationDTO> Applications { get; set; } = new();
    }

    public class DashboardStat
    {
        public string Label { get; set; } = null!;
        public int Value { get; set; }
    }

    public class EmployeeApplicationDTO
    {
        public int ApplicationId { get; set; }
        public JobDTO Job { get; set; } = null!;
        public EmployerInformationDTO Employer { get; set; } = null!;
        public int EmployeeId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime AppliedAt { get; set; }
    }

    public class JobDTO
    {
        public int JobId { get; set; }
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal JobSalaryMin { get; set; }
        public decimal JobSalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
    }

    public class EmployerInformationDTO
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; } = null!;
    }
}
