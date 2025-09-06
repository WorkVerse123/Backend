using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class EmployerProfileDTOResponse
    {
        public PaginatedResponse Paging { get; set; }

        public List<CompanyItemDTO> Companies { get; set; } = new();
    }
    public class CompanyItemDTO
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Industry { get; set; } = null!;
        public string? Website { get; set; }
        public string? Logo { get; set; }
    }
}
