using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class SendApplicationDTORequest
    {
        public int JobId { get; set; }
        public string? CoverLetter { get; set; }
    }

    public class UpdateApplicationStatusDTORequest
    {
        public string Status { get; set; } = null!;
    }
    public class ApplicationDTORequest
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public int EmployeeId { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } = null!;
        public DateTime AppliedAt { get; set; }
    }
}
