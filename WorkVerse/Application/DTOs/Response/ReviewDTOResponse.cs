using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // Danh sách review cho 1 job
    public class JobReviewListDTOResponse
    {
        public int JobId { get; set; }
        public PaginatedResponse Paging { get; set; }
        public List<JobReviewItemDTO> Reviews { get; set; } = new();
    }
    // Thông tin 1 review cụ thể
    public class JobReviewItemDTO
    {
        public int ReviewId { get; set; }
        public int EmployeeId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
