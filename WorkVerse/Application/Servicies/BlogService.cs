using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServicies;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BlogService> _logger;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BlogService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BlogDetailsDTOResponse> CreateBlogAsync(CreateBlogDTORequest request)
        {
            try
            {

                var author = await _unitOfWork.User.ExistByIdAsync(request.AuthorId);
                if (!author)
                {
                    throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found");
                }

                var blog = _mapper.Map<Blog>(request);

                var existSlug = await _unitOfWork.Blog.CheckBlogsExistBySlugAsync(blog.Slug);
                if (existSlug)
                {
                    throw new InvalidOperationException($"Blog with title slug has already exist");
                }

                await _unitOfWork.Blog.AddAsync(blog);
                await _unitOfWork.SaveChangesAsync();
                blog = await _unitOfWork.Blog.GetBlogDetailsByIdAsync(blog.BlogId);

                return _mapper.Map<BlogDetailsDTOResponse>(blog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating blogs with author {request.AuthorId}");
                throw;
            }
        }

        public async Task<BlogListDTOResponse> GetAllBlogListAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Blog.GetAllBlogListsAsync())
                            .AsQueryable();
                var totalRecords = query.Count();
                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending( c => c.CreatedAt)
                    .ToList();
                var mapped = _mapper.Map<List<BlogItemDTOResponse>>(pagedData);
                return new BlogListDTOResponse
                {
                    Blogs = mapped,
                    Paging = new PaginatedResponse
                    {
                        Page = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Blogs");
                throw;
            }
        }

        public async Task<BlogDetailsDTOResponse> GetBlogDetailsBySlugAsync(string slug)
        {
            try
            {
                var blog = await _unitOfWork.Blog.GetBlogDetailsBySlugAsync(slug);

                return _mapper.Map<BlogDetailsDTOResponse>(blog);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Blogs details by slug");
                throw;
            }
        }

        public async Task<BlogListDTOResponse> GetPublicBlogListAsync(int pageNumber, int pageSize)
        {
            try
            {
                var query = (await _unitOfWork.Blog.GetAllBlogListsAsync())
                            .AsQueryable();
                var totalRecords = query.Count();
                var pagedData = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Where(c => c.Status.ToLower() == "published")
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
                var mapped = _mapper.Map<List<BlogItemDTOResponse>>(pagedData);
                return new BlogListDTOResponse
                {
                    Blogs = mapped,
                    Paging = new PaginatedResponse
                    {
                        Page = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Blogs");
                throw;
            }
        }

        public async Task<bool> UpdateBlogAsync(int blogId, UpdateBlogDTORequest request)
        {
            try
            {
                var blog = await _unitOfWork.Blog.GetBlogByIdAsync(blogId);
                if (blog == null)
                {
                    throw new KeyNotFoundException($"Blog with ID {blogId} not found.");
                }
                _mapper.Map(request, blog);

                var existSlug = await _unitOfWork.Blog.CheckBlogsExistBySlugAsync(blog.Slug,blogId);
                if (existSlug)
                {
                    throw new InvalidOperationException($"Blog with title slug has already exist");
                }

                _unitOfWork.Blog.Update(blog);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status of Blog with ID: {Id}", blogId);
                throw;
            }
        }
    }
}
