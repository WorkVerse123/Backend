using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{

    // ứng viên xem danh sách hồ sơ ứng tuyển của chính mình
    public class JobApplicationListDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<JobApplicationItemDTO> Applications { get; set; } = new();
    }

    public class JobApplicationItemDTO
    {
        public int ApplicationId { get; set; }
		public int JobId { get; set; }
		public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string ApplicationStatus { get; set; } = null!;
    }
    // chi tiết 1 hồ sơ ứng tuyển
    public class JobApplicationDetailsDTOResponse   
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedAt { get; set; }
        public int EmployeeId { get; set; }
        public int JobId { get; set; }
        public string Status { get; set; } = null!;
    }

    // nhà tuyển dụng xem danh sách ứng viên cho 1 job

    public class EmployerJobApplicationsDTOResponse
    {
        public int EmployerId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public PaginatedResponse Paging { get; set; } = new();
        public List<JobApplicationSummaryDTO> Applications { get; set; } = new();
    }

    public class JobApplicationSummaryDTO
    {
        public int ApplicationId { get; set; }
        public DateTime AppliedAt { get; set; }
        public string EmployeeFullName { get; set; } = null!;
        public string EmployeeGender { get; set; } = null!;
    }

    // thống kê chung của hệ thống 
    public class PlatformStatsResponseDTOResponse
    {
        public PlatformStatsDTO Stats { get; set; } = new();
    }
    public class PlatformStatsDTO
    {
        public int Jobs { get; set; }
        public int Companies { get; set; }
        public int Candidates { get; set; }
        public int NewJobs { get; set; }
    }
}
