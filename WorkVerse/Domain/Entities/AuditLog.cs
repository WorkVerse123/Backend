using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class AuditLog
    {
        public int LogId { get; set; }
        public int ActorId { get; set; }
        public string Action { get; set; } = null!;
        public string TargetTable { get; set; } = null!;
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual User Actor { get; set; } = null!;
    }
}
