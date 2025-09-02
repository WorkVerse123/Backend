using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ApplicationResponseDTO
    {
        public List<ApplicationItemDTO> Applications { get; set; } = new();
    }

    public class ApplicationItemDTO
    {
        public int ApplicationId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
        public string ApplicationStatus { get; set; } = null!;
    }
}
