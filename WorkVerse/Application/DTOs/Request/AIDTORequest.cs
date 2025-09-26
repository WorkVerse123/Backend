using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class JobQuery
    {
        public List<string>? Title { get; set; }
        public List<string>? Location { get; set; }
        public List<string>? Categories { get; set; }   // F&B, Retail, ...
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public List<string>? JobTime { get; set; } // ["full-time"], ["part-time"], hoặc ["full-time","part-time"]
        public List<byte>? DaysOfWeek { get; set; } // 0=Sunday, 1=Monday...
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }

    public class EmployerQuery
    {
        public List<string>? CompanyNames { get; set; }
        public List<string>? EmployerTypes { get; set; }  // F&B, Bán lẻ, ...
        public List<string>? Address { get; set; }
    }

    public class EmployeeQuery
    {
        public List<string>? FullName { get; set; }
        public List<string>? Skills { get; set; }
        public List<string>? Education { get; set; }
        public List<string>? Gender { get; set; }  // ["Male"], ["Female"], hoặc cả 2
        public List<string>? WorkExperience { get; set; }
        public List<string>? Address { get; set; }
    }

    public class AIQueryResult
    {
        public JobQuery? Job { get; set; }
        public EmployerQuery? Employer { get; set; }
        public EmployeeQuery? Employee { get; set; }
    }
}
