using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ListEmployerProfileDTOResponse
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
    public class EmployerProfileDTOResponse
    {
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int EmployerTypeId { get; set; }
        public string Address { get; set; } = null!;
        public string? WebsiteUrl { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime? DateEstablish { get; set; }
        public string Description { get; set; } = null!;
        public string? SearchName { get; set; }
    }
}
