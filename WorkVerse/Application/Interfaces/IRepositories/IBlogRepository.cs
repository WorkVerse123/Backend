using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetAllBlogListsAsync();

        Task<Blog> GetBlogDetailsBySlugAsync(string slug);

        Task<Blog> GetBlogDetailsByIdAsync(int blogId);

        Task<bool> CheckBlogsExistBySlugAsync(string slug, int? excludeId = null);
        Task<Blog?> GetBlogByIdAsync(int blogId);

    }
}
