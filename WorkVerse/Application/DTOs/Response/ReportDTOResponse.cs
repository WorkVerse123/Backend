using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ReportDetailsDTOResponse
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string TargetType { get; set; } = null!;
        public int TargetId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
    }
    // danh sach report cho admin
    public class ReportListDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<ReportIncludeReporterDTOResponse> Reports { get; set; } = new();

    }

    public class ReportIncludeReporterDTOResponse
    {
        public int ReportId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
        public ReporterDTOResponse Reporter { get; set; } = null!;
        public ReportTargetDTOResponse Target { get; set; } = null!;
    }
    public class ReporterDTOResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
    }
    public class ReportTargetDTOResponse
    {
        public string Type { get; set; } = null!;
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
    public class ReportDTORespone
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
