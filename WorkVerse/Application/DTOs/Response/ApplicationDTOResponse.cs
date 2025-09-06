using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ApplicationResponseDTO
    {
        public PaginatedResponse Paging { get; set; }
        public List<ApplicationItemDTO> Applications { get; set; } = new();
    }

    public class ApplicationItemDTO
    {
        public int ApplicationId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string ApplicationStatus { get; set; } = null!;
    }

    public class ApplicationItemDetailDTO
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedAt { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public string Status { get; set; } = null!;
    }

    public class JobApplicationsResponseDTO
    {
        public int EmployerId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public PaginatedResponse Paging { get; set; } = new();
        public List<ApplicationSummaryDTO> Applications { get; set; } = new();
    }

    public class ApplicationSummaryDTO
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedAt { get; set; }
        public string EmployeeFullName { get; set; } = null!;
        public string EmployeeGender { get; set; } = null!;
    }

    public class StatsInformationDTOResponse
    {
        public StatItemDTO Stats { get; set; } = new();
    }
    public class StatItemDTO
    {
        public int Jobs { get; set; }
        public int Companies { get; set; }
        public int Candidates { get; set; }
        public int NewJobs { get; set; }
    }
}
