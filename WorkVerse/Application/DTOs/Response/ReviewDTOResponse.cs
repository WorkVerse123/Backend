using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ReviewDTOResponse
    {
        public int JobId { get; set; }
        public PaginatedResponse Paging { get; set; }
        public List<ReviewItemDTO> Reviews { get; set; } = new();
    }
    public class ReviewItemDTO
    {
        public int ReviewId { get; set; }
        public int EmployeeId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
