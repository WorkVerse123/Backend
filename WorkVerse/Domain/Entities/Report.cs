using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ReportedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
