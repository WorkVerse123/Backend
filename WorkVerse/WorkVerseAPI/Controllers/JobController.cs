using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("jobs")]
    public class JobController : ControllerBase
    {
        // This is a placeholder for the actual implementation of user service
        private readonly IEmployeeProfileServices _employeeProfileService;
        private readonly IBusyTimeService _busyTimeService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IApplicationService _applicationService;
        private readonly IJobService _jobService;
        private readonly IReviewService _reviewService;
        private readonly IJobCategoryService _jobCategoryService;
        public JobController(IEmployeeProfileServices employeeProfileService, IBusyTimeService busyTimeService, IBookmarkService bookmarkService, IApplicationService applicationService, IJobService jobService, IReviewService reviewService, IJobCategoryService jobCategoryService)
        {
            _employeeProfileService = employeeProfileService;
            _busyTimeService = busyTimeService;
            _bookmarkService = bookmarkService;
            _applicationService = applicationService;
            _jobService = jobService;
            _reviewService = reviewService;
            _jobCategoryService = jobCategoryService;
        }

        // GET /jobs
        [HttpGet]
        public async Task<IActionResult> GetAllJobs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _jobService.GetAllAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobDTOResponse>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /jobs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById([FromRoute] int id)
        {
            try
            {
                var result = await _jobService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobItemDetailDTO>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /categories
        [HttpGet("/categories")] // GET api/categories
        public async Task<IActionResult> GetAllJobCategories()
        {
            try
            {
                var result = await _jobCategoryService.GetAllJobCategoriesAsync();
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobCategoryDTOResponse>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /jobs/{id}/reviews
        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviewsByJobId([FromRoute] int id,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _reviewService.GetByJobIdAsync(id,pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<ReviewDTOResponse>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /jobs/{id}/reviews
        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> CreateReview([FromRoute] int id, [FromBody] ReviewDTORequest request)
        {
            try
            {
                var (isValid, error) = ReviewValidationHelper.ValidateReviewPostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _reviewService.CreateReviewAsync(id, request);
                return Ok(new ApiResponse<ReviewItemDTO>("Review created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
