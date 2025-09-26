using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class CreateBlogDTORequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public int AuthorId { get; set; }
        public string Status { get; set; } = null!;

    }

    public class UpdateBlogDTORequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Status { get; set; } = null!;

    }
}
