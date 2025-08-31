using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Application
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } = null!;
        public DateTime AppliedAt { get; set; }

        public virtual Job Job { get; set; } = null!;
        public virtual EmployeeProfile Employee { get; set; } = null!;
    }
}
