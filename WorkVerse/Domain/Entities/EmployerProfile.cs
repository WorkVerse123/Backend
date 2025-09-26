using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class EmployerProfile
    {
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int EmployerTypeId { get; set; }
        public string Address { get; set; } = null!;
        public string? WebsiteUrl { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime? DateEstablish { get; set; }
        public string Description { get; set; } = null!;
        public string? SearchName { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual EmployerType EmployerType { get; set; } = null!;
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
