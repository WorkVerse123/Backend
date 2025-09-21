using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // Danh sách job 
    public class JobListDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<JobSummaryDTO> Jobs { get; set; } = new();
    }
    public class JobSummaryDTO
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string JobLocation { get; set; } = null!;
        public decimal JobSalaryMin { get; set; }
        public decimal JobSalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
        public bool IsPriority { get; set; } = false;
        public DateTime JobCreatedAt { get; set; }
        public DateTime JobExpiredAt { get; set; }
        public string JobStatus { get; set; } = null!;
    }

    public class JobDetailsDTOResponse
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string JobDescription { get; set; } = null!;
        public string? JobRequirements { get; set; }
        public string JobLocation { get; set; } = null!;
        public decimal JobSalaryMin { get; set; }
        public decimal JobSalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
        public bool IsPriority { get; set; } = false;

        public DateTime JobCreatedAt { get; set; }
        public DateTime JobExpiredAt { get; set; }
        public string JobStatus { get; set; } = null!;
    }
}
