using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/employer")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerProfileService _employerProfileService;
        private readonly IJobService _jobService;
        private readonly IApplicationService _applicationService;
        private readonly IReviewService _reviewService;
        private readonly IJobCategoryService _jobCategoryService;

        public EmployerController(
            IEmployerProfileService employerProfileService,
            IJobService jobService,
            IApplicationService applicationService,
            IReviewService reviewService,
            IJobCategoryService jobCategoryService)
        {
            _employerProfileService = employerProfileService;
            _jobService = jobService;
            _applicationService = applicationService;
            _reviewService = reviewService;
            _jobCategoryService = jobCategoryService;
        }

        // GET /employers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employer ID must be greater than 0", 400));
                }

                var employerProfile = await _employerProfileService.GetEmployerProfileByIdAsync(id);
                if (employerProfile == null)
                {
                    return NotFound(new ApiResponse<object>($"Employer with ID {id} not found.", 404));
                }

                return Ok(new ApiResponse<EmployerProfileDTOResponse>("Employer profile retrieved successfully.", employerProfile, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /employers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployer(int id, [FromBody] EmployerProfileDTORequest employerDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employer ID must be greater than 0", 400));
                }

                var (isValid, error) = EmployerProfileValidationHelper.ValidateEmployerProfilePostRequest(employerDto);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse<object>(error, 400));
                }

                var result = await _employerProfileService.UpdateEmployerProfileAsync(id, employerDto);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>($"Employer with ID {id} not found.", 404));
                }

                return Ok(new ApiResponse<object>("Employer profile updated successfully.", null, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /employers/{id}/jobs
        [HttpGet("{id}/jobs")]
        public async Task<IActionResult> GetJobsByEmployer(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employer ID must be greater than 0", 400));
                }

                var jobsResponse = await _jobService.GetJobsByEmployerIdAsync(id, pageNumber, pageSize);
                if (jobsResponse == null || jobsResponse.Jobs == null || jobsResponse.Jobs.Count == 0)
                {
                    return NotFound(new ApiResponse<object>($"No jobs found for employer with ID {id}.", 404));
                }

                return Ok(new ApiResponse<JobListDTOResponse>("Jobs retrieved successfully.", jobsResponse, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /employers/{id}/jobs
        [HttpPost("{id}/jobs")]
        public async Task<IActionResult> CreateJobForEmployer(int id, [FromBody] JobDTORequest jobDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employer ID must be greater than 0", 400));
                }

                var (isValid, error) = JobValidationHelper.ValidateJobRequest(jobDto);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse<object>(error, 400));
                }

                // Ensure the employerId is set in the DTO
                jobDto.EmployerId = id;

                await _jobService.AddJobAsync(jobDto);

                return Ok(new ApiResponse<object>("Job created successfully.", null, 201));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /employers/{id}/jobs/{job_id}
        [HttpPut("{id}/jobs/{job_id}")]
        public async Task<IActionResult> UpdateJobForEmployer(int id, int job_id, [FromBody] JobDTORequest jobDto)
        {
            try
            {
                if (id <= 0 || job_id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employer ID and Job ID must be greater than 0", 400));
                }

                var (isValid, error) = JobValidationHelper.ValidateJobRequest(jobDto);
                if (!isValid)
                {
                    return BadRequest(new ApiResponse<object>(error, 400));
                }

                // Ensure the employerId is set in the DTO
                jobDto.EmployerId = id;

                var updated = await _jobService.UpdateJobAsync(job_id, jobDto);
                if (!updated)
                {
                    return NotFound(new ApiResponse<object>($"Job with ID {job_id} for employer {id} not found.", 404));
                }

                return Ok(new ApiResponse<object>("Job updated successfully.", null, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        //// PUT /employers/{id}/jobs/{job_id}/status
        //[HttpPut("{id}/jobs/{job_id}/status")]
        //public async Task<IActionResult> UpdateJobStatus(int id, int job_id, [FromBody] string statusDto)
        //{
        //    try
        //    {
        //        if (id <= 0 || job_id <= 0)
        //        {
        //            return BadRequest(new ApiResponse<object>("Employer ID and Job ID must be greater than 0", 400));
        //        }

        //        if (string.IsNullOrWhiteSpace(statusDto))
        //        {
        //            return BadRequest(new ApiResponse<object>("Status is required.", 400));
        //        }

        //        var allowedStatus = new[] { "Open", "Closed", "Draft" };
        //        if (!allowedStatus.Contains(statusDto))
        //        {
        //            return BadRequest(new ApiResponse<object>($"Status must be one of: {string.Join(", ", allowedStatus)}.", 400));
        //        }

        //        var job = await _jobService.GetByIdAsync(job_id);
        //        if (job == null || job.JobId != job_id || job.EmployerId != id)
        //        {
        //            return NotFound(new ApiResponse<object>($"Job with ID {job_id} for employer {id} not found.", 404));
        //        }

        //        var updated = await _jobService.ChangeStatusAsynce(job_id, statusDto);
        //        if (!updated)
        //        {
        //            return StatusCode(500, new ApiResponse<object>("Failed to update job status.", 500));
        //        }

        //        return Ok(new ApiResponse<object>("Job status updated successfully.", null, 200));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiResponse<object>(ex.Message, 404));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
        //    }
        //}

        //// PUT /applications/{id}/status
        //[HttpPut("/applications/{id}/status")]
        //public IActionResult UpdateApplicationStatus(int id, [FromBody] object statusDto)
        //{
        //    // TODO: Update application status by id
        //    return NoContent();
        //}
    }
}
