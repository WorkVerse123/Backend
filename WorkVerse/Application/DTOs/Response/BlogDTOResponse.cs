using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class BlogListDTOResponse
    {
        public PaginatedResponse Paging { get; set; } = null!;
        public List<BlogItemDTOResponse> Blogs { get; set; } = new();

    }

    public class BlogItemDTOResponse    
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = null!;
        public string Excerpt { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;


    }

    public class BlogDetailsDTOResponse
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public AuthorBlogDTOResponse Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } = null!; 

    }
    public class AuthorBlogDTOResponse
    {
        public int AuthorId { get; set; }
        public string fullName { get; set; }
    }
}
