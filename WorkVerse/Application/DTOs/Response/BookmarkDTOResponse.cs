using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class BookmarkDTOResponse
    {
        public int EmployeeId { get; set; }
        public List<BookmarkItemDTO> Bookmarks { get; set; } = new();
    }
    public class BookmarkItemDTO
    {
        public int BookmarkId { get; set; }
        public string JobTitle { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public List<string> JobCategories { get; set; } = new();
    }

}
