using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class JobDTORequest
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Requirements { get; set; }
        public string Location { get; set; } = null!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public string JobTime { get; set; } = null!;
        public bool IsPriority { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Status { get; set; } = null!;
    }
}
