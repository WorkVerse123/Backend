using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/jobs")]
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
                var result = await _jobService.GetJobListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobListDTOResponse>("Registration successful", result, 200));
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
                var result = await _jobService.GetJobByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobDetailsDTOResponse>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /categories
        [HttpGet("categories")] // GET api/categories
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

                return Ok(new ApiResponse<JobCategoryListDTOResponse>("Registration successful", result, 200));
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
                var result = await _reviewService.GetJobReviewsByJobIdAsync(id,pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<JobReviewListDTOResponse>("Registration successful", result, 200));
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
                var result = await _reviewService.CreateJobReviewAsync(id, request);
                return Ok(new ApiResponse<JobReviewItemDTO>("Review created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET/candidates
        [HttpGet("cadidates")]
        public async Task<IActionResult> GetAllCandidates([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _employeeProfileService.GetEmployeeListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<CandidateListDTOResponse>("Get list of candidates successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
