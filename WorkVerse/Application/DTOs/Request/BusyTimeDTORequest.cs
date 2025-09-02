using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class BusyTimeDTORequest
    {
        public List<EmployeeBusyTimesDTORequest> BusyTimes { get; set; } = new();
    }

    public class EmployeeBusyTimesDTORequest
    {
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
