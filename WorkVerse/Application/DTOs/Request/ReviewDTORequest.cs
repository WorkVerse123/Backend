using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class ReviewDTORequest
    {
        public int EmployeeId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
