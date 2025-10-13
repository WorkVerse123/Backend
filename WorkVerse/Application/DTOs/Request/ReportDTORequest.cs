using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class SubmitReportDTORequest
    {
        public int UserId { get; set; }
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public string Reason { get; set; } = null!;
    }
    public class UpdateReportStatusDTORequest
    {   
        public string Status { get; set; } = null!;
    }
    public class ReportDTORequest
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
    }
 }
