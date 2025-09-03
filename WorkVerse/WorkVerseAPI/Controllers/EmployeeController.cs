using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{

    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        // This is a placeholder for the actual implementation of user service
        private readonly IEmployeeProfileServices _employeeProfileService;
        private readonly IBusyTimeService _busyTimeService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IApplicationService _applicationService;
        public EmployeeController(IEmployeeProfileServices employeeProfileService, IBusyTimeService busyTimeService, IBookmarkService bookmarkService, IApplicationService applicationService)
        {
            _employeeProfileService = employeeProfileService;
            _busyTimeService = busyTimeService;
            _bookmarkService = bookmarkService;
            _applicationService = applicationService;
        }
        // POST /employees/{id}
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateProfile([FromRoute] int userId,[FromBody] EmployeeProfileDTORequest request)
        {
            try
            {
                var (isValid, error) = EmployeeProfileValidationHelper.ValidateEmployeeProfileRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _employeeProfileService.CreateProfileAsync(userId,request);
                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
        // GET /employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {           
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employee ID must be greater than 0", 400));
                }

                var result = await _employeeProfileService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>($"No profile found with ID {id}", 404));
                }

                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
        // PUT /employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] EmployeeProfileDTORequest request, [FromRoute] int id)
        {
            try
            {
                var (isValid, error) = EmployeeProfileValidationHelper.ValidateEmployeeProfileRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                if (id == null)
                {
                    return BadRequest(new ApiResponse<object>("EmployeeId mismatch", 400));
                }

                var result = await _employeeProfileService.UpdateProfileAsync(id,request);
                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile updated successfully", result, 200));
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

        // GET /employees/{id}/busy-times
        [HttpGet("{id}/busy-times")]
        public async Task<IActionResult> GetBusyTimes([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _busyTimeService.GetByEmployeeIdAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound(new ApiResponse<object>(
                        $"No busy times found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<IEnumerable<BusyTimeDTOResponse>>("Busy times retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message,500));
            }
        }

        // POST /employees/{id}/busy-times

        [HttpPost("{id}/busy-times")]
        public async Task<IActionResult> CreateBusyTimes([FromRoute] int id, [FromBody] BusyTimeDTORequest request)
        {
            try
            {
                var (isValid, error) = BusyTimeValidationHelper.ValidateBusyTimePostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));

                var result = await _busyTimeService.CreateBusyTimesAsync(id, request);
                return Ok(new ApiResponse<IEnumerable<BusyTimeDTOResponse>>("BusyTime created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /employees/{id}/busy-times/{busy_time_id}

        [HttpPut("{id}/busy-times")]
        public async Task<IActionResult> UpdateBusyTime([FromBody] BusyTimeDTORequest request, [FromRoute] int id)
        {
            try
            {
                var (isValid, error) = BusyTimeValidationHelper.ValidateBusyTimePutRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
             
                var result = await _busyTimeService.UpdateBusyTimesAsync(id, request);
                return Ok(new ApiResponse<IEnumerable<BusyTimeDTOResponse>>("BusyTime updated successfully", result, 200));
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

        // DELETE /employees/{id}/busy-times/{busy_time_id}
        [HttpDelete("{id}/busy-times/{busy_time_id}")]
        public async Task<IActionResult> DeleteBusyTime([FromRoute] int id, [FromRoute] int busy_time_id)
        {
            try
            {
                if (id <= 0 || busy_time_id <= 0)
                    return BadRequest(new ApiResponse<object>("Invalid EmployeeId or BusyTimeId", 400));

                var result = await _busyTimeService.DeleteBusyTimesAsync(id, busy_time_id);

                return Ok(new ApiResponse<object>("BusyTime deleted successfully", 200));
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

        // GET /employees/{id}/bookmarks
        [HttpGet("{id}/bookmarks")]
        public async Task<IActionResult> GetBookmark([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _bookmarkService.GetByEmployeeIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Bookmark found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<BookmarkDTOResponse>("Busy times retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /employees/{id}/bookmarks/jobs/{job_id}
        [HttpPost("{id}/bookmarks/jobs/{job_id}")]
        public async Task<IActionResult> CreateBookmark([FromRoute] int id, [FromRoute] int job_id)
        {
            try
            {
                var result = await _bookmarkService.CreateBookmarkAsync(id, job_id);
                return Ok(new ApiResponse<BookmarkItemDTO>("Bookmark created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // DELETE /employees/{id}/bookmarks/{bookmark_id}
        [HttpDelete("{id}/bookmarks/{bookmark_id}")]
        public async Task<IActionResult> DeleteBookmark([FromRoute] int id, [FromRoute] int bookmark_id)
        {
            if (id <= 0 || bookmark_id <= 0)
                return BadRequest(new ApiResponse<object>("Invalid EmployeeId or BookmarkId", 400));

            try
            {
                var result = await _bookmarkService.DeleteBookmarkAsync(id, bookmark_id);

                return Ok(new ApiResponse<object>("Bookmark deleted successfully", 200));
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

        // GET /employees/{id}/applications
        [HttpGet("{id}/applications")]
        public async Task<IActionResult> GetApplications([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _applicationService.GetByEmployeeIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Application found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<ApplicationResponseDTO>("Applications retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }

}
