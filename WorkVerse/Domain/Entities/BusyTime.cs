using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class BusyTime
    {
        public int BusyTimeId { get; set; }
        public int EmployeeId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual EmployeeProfile Employee { get; set; } = null!;
    }
}
