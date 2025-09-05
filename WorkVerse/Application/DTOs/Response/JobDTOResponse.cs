using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class JobDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<JobItemDTO> Jobs { get; set; } = new();
    }
    public class JobItemDTO
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string JobLocation { get; set; } = null!;
        public decimal JobSalaryMin { get; set; }
        public decimal JobSalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
        public DateTime JobCreatedAt { get; set; }
        public DateTime JobExpiredAt { get; set; }
        public string JobStatus { get; set; } = null!;
    }
}
