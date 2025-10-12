using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // danh sách ứng viên
    public class CandidateListDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<CandidateItemDTO> Candidates { get; set; } = new();
    }
    public class CandidateItemDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? EmployeeLocation { get; set; }
        public string? EmployeeEducation { get; set; }
        public string? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public string? WorkExperience { get; set; }
        public bool IsPriority { get; set; } = false;

    }
}


