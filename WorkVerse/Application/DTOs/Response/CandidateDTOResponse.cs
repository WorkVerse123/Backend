using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class CandidateDTOResponse
    {
        public PaginatedResponse Paging { get; set; }
        public List<CandidateItemDTO> Candidates { get; set; } = new();
    }
    public class CandidateItemDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string? EmployeeLocation { get; set; }
        public string? EmployeeEducation { get; set; }
        public string? Gender { get; set; }
    }
}
