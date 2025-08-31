using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Job
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
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Status { get; set; } = null!;
        public string? SearchName { get; set; }

        public virtual EmployerProfile Employer { get; set; } = null!;
        public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
        public virtual ICollection<JobCategoryMapping> JobCategoryMappings { get; set; } = new List<JobCategoryMapping>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
