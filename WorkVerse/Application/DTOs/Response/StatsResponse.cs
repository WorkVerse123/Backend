using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class StatsResponse
    {
        public int TotalUsers { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalEmployers { get; set; }
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }
        public int TotalReports { get; set; }
        public int TotalFeedbacks { get; set; }

    }
}
