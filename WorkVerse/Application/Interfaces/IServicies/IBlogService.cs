using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServicies
{
    public interface IBlogService
    {
        Task<BlogListDTOResponse> GetAllBlogListAsync( int pageNumber, int pageSize);
        Task<BlogListDTOResponse> GetPublicBlogListAsync( int pageNumber, int pageSize);
        Task<BlogDetailsDTOResponse> GetBlogDetailsBySlugAsync(string slug);
        Task<BlogDetailsDTOResponse> CreateBlogAsync(CreateBlogDTORequest request);
        Task<bool> UpdateBlogAsync(int blogId, UpdateBlogDTORequest request);


    }
}
