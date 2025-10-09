using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{

    [ApiController]
    [Route("api/employees")]
    [Authorize]
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
        [Authorize(Roles = "4")]

        public async Task<IActionResult> CreateProfile([FromRoute] int userId,[FromBody] EmployeeProfileDTORequest request)
        {
            try
            {
                var (isValid, error) = EmployeeProfileValidationHelper.ValidateEmployeeProfileRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _employeeProfileService.CreateEmployeeProfileAsync(userId,request);
                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile created successfully", result, 201));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new ApiResponse<object>(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
        // GET /employees/{id}
        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {           
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Employee ID must be greater than 0", 400));
                }

                var result = await _employeeProfileService.GetEmployeeProfileByIdAsync(id);
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
        [Authorize(Roles = "4")]
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

                var result = await _employeeProfileService.UpdateEmployeeProfileAsync(id,request);
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
        [Authorize(Roles = "4")]
        public async Task<IActionResult> GetBusyTimes([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _busyTimeService.GetBusyTimesByEmployeeAsync(id);
                if (result == null || !result.Any())
                {
                    return NotFound(new ApiResponse<object>(
                        $"No busy times found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<IEnumerable<BusyTimeItemDTO>>("Busy times retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message,500));
            }
        }

        // POST /employees/{id}/busy-times

        [HttpPost("{id}/busy-times")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> CreateBusyTimes([FromRoute] int id, [FromBody] BusyTimeDTORequest request)
        {
            try
            {
                var (isValid, error) = BusyTimeValidationHelper.ValidateBusyTimePostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));

                var result = await _busyTimeService.AddBusyTimesAsync(id, request);
                return Ok(new ApiResponse<IEnumerable<BusyTimeItemDTO>>("BusyTime created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /employees/{id}/busy-times/{busy_time_id}

        [HttpPut("{id}/busy-times")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> UpdateBusyTime([FromBody] BusyTimeDTORequest request, [FromRoute] int id)
        {
            try
            {
                var (isValid, error) = BusyTimeValidationHelper.ValidateBusyTimePutRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
             
                var result = await _busyTimeService.UpdateBusyTimesByEmployeeAsync(id, request);
                return Ok(new ApiResponse<IEnumerable<BusyTimeItemDTO>>("BusyTime updated successfully", result, 200));
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
        [Authorize(Roles = "4")]

        public async Task<IActionResult> DeleteBusyTime([FromRoute] int id, [FromRoute] int busy_time_id)
        {
            try
            {
                if (id <= 0 || busy_time_id <= 0)
                    return BadRequest(new ApiResponse<object>("Invalid EmployeeId or BusyTimeId", 400));

                var result = await _busyTimeService.RemoveBusyTimeAsync(id, busy_time_id);

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
        [Authorize(Roles = "4")]

        public async Task<IActionResult> GetBookmark([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _bookmarkService.GetBookmarksByEmployeeAsync(id, pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Bookmark found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<JobBookmarkListDTOResponse>("Busy times retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /employees/{id}/bookmarks/jobs/{job_id}
        [HttpPost("{id}/bookmarks/jobs/{job_id}")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> CreateBookmark([FromRoute] int id, [FromRoute] int job_id)
        {
            try
            {
                var result = await _bookmarkService.AddBookmarkAsync(id, job_id);
                return Ok(new ApiResponse<JobBookmarkItemDTO>("Bookmark created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // DELETE /employees/{id}/bookmarks/{bookmark_id}
        [HttpDelete("{id}/bookmarks/{bookmark_id}")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> DeleteBookmark([FromRoute] int id, [FromRoute] int bookmark_id)
        {
            if (id <= 0 || bookmark_id <= 0)
                return BadRequest(new ApiResponse<object>("Invalid EmployeeId or BookmarkId", 400));

            try
            {
                var result = await _bookmarkService.RemoveBookmarkAsync(id, bookmark_id);

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
        [Authorize(Roles = "4")]

        public async Task<IActionResult> GetApplications([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>(
                        "Employee ID must be greater than 0", 400));
                }

                var result = await _applicationService.GetApplicationsByEmployeeAsync(id, pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Application found for employee with ID {id}", 404));
                }

                return Ok(new ApiResponse<JobApplicationListDTOResponse>("Applications retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /employees/{id}/applications
        [HttpPost("{id}/applications")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> CreateApplication([FromRoute] int id, [FromBody] SendApplicationDTORequest request)
        {
            try
            {
                var (isValid, error) = ApplicationValidationHelper.ValidateApplicationPostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));

                var result = await _applicationService.CreateApplyJobAsync(id, request);
                return Ok(new ApiResponse<JobApplicationItemDTO>("Application created successfully", result, 201));
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

        // GET/employee-dashboard/{id}
        [HttpGet("/employee-dashboard/{id}")]
        [Authorize(Roles = "4")]

        public async Task<IActionResult> GetEmployeeDashBoardById([FromRoute] int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {

                var result = await _employeeProfileService.GetEmployeeDashBoardAsync(id,pageNumber,pageSize);
                
                return Ok(new ApiResponse<EmployeeDashboardDTOResponse>("Employee dashboard data (mock)", result, 200));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 400));
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
    }

}
