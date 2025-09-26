using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET/blogs-published
        [HttpGet("/api/blogs-published")]
        public async Task<IActionResult> GetPublicBlogList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _blogService.GetPublicBlogListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<BlogListDTOResponse>("Get list of Blogs successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET/blogs
        [HttpGet]
        public async Task<IActionResult> GetAllBlogList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _blogService.GetAllBlogListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<BlogListDTOResponse>("Get list of Blogs successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET/blogs/{slug}
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBlogDetailsBySlug([FromRoute] string slug)
        {
            try
            {
                var result = await _blogService.GetBlogDetailsBySlugAsync(slug);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<BlogDetailsDTOResponse>("Get list of Blogs details successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /blogs
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateBlogDTORequest request)
        {
            try
            {
                var (isValid, error) = BlogValidationHelper.ValidationCreateBlogRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _blogService.CreateBlogAsync(request);
                return Ok(new ApiResponse<BlogDetailsDTOResponse>("Blogs created successfully", result, 201));
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(404, new ApiResponse<object>(ex.Message, 404));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(404, new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /blogs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog([FromRoute] int id, [FromBody] UpdateBlogDTORequest request)
        {
            try
            {
                var (isValid, error) = BlogValidationHelper.ValidationUpdateBlogRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var updated = await _blogService.UpdateBlogAsync(id, request);
                if (!updated)
                {
                    return StatusCode(500, new ApiResponse<object>("Update blog failed", 500));
                }

                return Ok(new ApiResponse<object>("Blog updated successfully.", null, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 404));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
