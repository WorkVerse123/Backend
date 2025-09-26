using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    // danh sách bookmark job của employee
    public class JobBookmarkListDTOResponse
    {
        public int EmployeeId { get; set; }
        public PaginatedResponse Paging { get; set; }
        public List<JobBookmarkItemDTO> Bookmarks { get; set; } = new();
    }
    public class JobBookmarkItemDTO
    {
        public int BookmarkId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public List<string> JobCategory { get; set; } = new();
    }

}
