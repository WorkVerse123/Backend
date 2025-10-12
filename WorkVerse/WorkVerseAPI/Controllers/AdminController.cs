using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        private readonly IStaffProfileService _staffProfileService;
        private readonly IUserService _userService;
        private readonly IEmployeeProfileServices _employeeService;
        private readonly IEmployerProfileService _employerService;
        private readonly IReportService _reportService;
        private readonly IFeedbackService _feedbackService;
        private readonly IJobService _jobService;
        private readonly IApplicationService _applicationService;

        public AdminController(
            ILogger<AdminController> logger,
            IStaffProfileService staffProfileService,
            IUserService userService,
            IEmployeeProfileServices employeeService,
            IEmployerProfileService employerService,
            IReportService reportService,
            IFeedbackService feedbackService,
            IJobService jobService,
            IApplicationService applicationService,
            IAdminService adminService
        )
        {
            _logger = logger;
            _staffProfileService = staffProfileService;
            _userService = userService;
            _employeeService = employeeService;
            _employerService = employerService;
            _reportService = reportService;
            _feedbackService = feedbackService;
            _jobService = jobService;
            _applicationService = applicationService;
            _adminService = adminService;
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] StaffProfileDTORequest staffProfile)
        {
            var result = await _staffProfileService.Create(staffProfile);
            if (result == null)
                return BadRequest(new ApiResponse<object>("Failed to create staff profile.", 400));

            return Ok(new ApiResponse<object>("Create staff profile success!", 200));
        }

        [HttpPut("user/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UserUpdateStatusDTORequest userUpdate)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse<object>("User ID must be greater than 0", 400));

                var result = await _userService.UpdateStatusAsync(id, userUpdate.NewStatus);
                if (result == null)
                    return NotFound(new ApiResponse<object>($"User with ID {id} not found or status not updated.", 404));

                return Ok(new ApiResponse<object>("User status updated successfully.", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for user id {UserId}", id);
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }


        [HttpGet("users")]
        public async Task<IActionResult>? GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list user successfully", result, 200));
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTORequest userDto)
        {
            var result = await _userService.UpdateAsync(userDto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"User with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("User updated successfully.", 200));
        }


        [HttpGet("employee")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _employeeService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list employee successfully", result, 200));

        }

        [HttpPut("employee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeProfileUpdateDTORequest dto)
        {
            var result = await _employeeService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Employee with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Employee updated successfully.", 200));
        }


        [HttpGet("employer")]
        public async Task<IActionResult> GetAllEmployers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _employerService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list employer successfully", result, 200));
        }

        [HttpPut("employer/{id}")]
        public async Task<IActionResult> UpdateEmployer(int id, [FromBody] EmployerProfileDTORequest dto)
        {
            var result = await _employerService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Employer with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Employer updated successfully.", 200));
        }

        //// ======================================================
        //// STAFF
        //// ======================================================
        [HttpGet("staff")]
        public async Task<IActionResult> GetAllStaffs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _staffProfileService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list staff successfully", result, 200));

        }

        [HttpPut("staff/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] StaffProfileDTORequest dto)
        {
            var result = await _staffProfileService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Staff with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Staff updated successfully.", 200));
        }


        [HttpGet("reports")]
        public async Task<IActionResult> GetAllReports([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reportService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list reports successfully", result, 200));
        }

        [HttpPut("reports/{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ReportDTORequest dto)
        {
            var result = await _reportService.UpdateAsync( dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Report with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Report updated successfully.", 200));
        }


        [HttpGet("feedbacks")]
        public async Task<IActionResult> GetAllFeedbacks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _feedbackService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list feedbacks successfully", result, 200));
        }

        [HttpPut("feedbacks/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackDTORequest dto)
        {
            var result = await _feedbackService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Feedback with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Feedback updated successfully.", 200));
        }


        [HttpGet("jobs")]
        public async Task<IActionResult> GetAllJobs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _jobService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list jobs successfully", result, 200));
        }

        [HttpPut("jobs/{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDTORequest dto)
        {
            var result = await _jobService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Job with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Job updated successfully.", 200));
        }


        [HttpGet("applications")]
        public async Task<IActionResult> GetAllApplications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _applicationService.GetAllAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<object>("Get list applications successfully", result, 200));
        }

        [HttpPut("applications/{id}")]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] ApplicationDTORequest dto)
        {
            var result = await _applicationService.UpdateAsync(dto);
            if (result == null)
                return NotFound(new ApiResponse<object>($"Application with ID {id} not found.", 404));

            return Ok(new ApiResponse<object>("Application updated successfully.", 200));
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetSystemStats()
        {
            try
            {
                var result = await _adminService.GetSystemStatsAsync();
                if (result == null)
                    return NotFound(new ApiResponse<object>("Failed to retrieve system stats.", 404));

                return Ok(new ApiResponse<StatsResponse>("Get system stats successfully.", result, 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving system stats");
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChartData([FromQuery] DateOnly? startDate = null, [FromQuery] DateOnly? endDate = null)
        {
            try
            {
                var start = startDate ?? DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));
                var end = endDate ?? DateOnly.FromDateTime(DateTime.Now);

                var result = await _adminService.GetChartsAsync(start, end);
                if (result == null)
                    return NotFound(new ApiResponse<object>("Failed to retrieve chart data.", 404));

                return Ok(new ApiResponse<ChartResponse>("Get chart data successfully.", result, 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chart data");
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        [HttpPost("payments/filter")]
        public async Task<IActionResult> FilterPayments([FromBody] PaymentFilterDTORequest filter, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _adminService.FilterPayment(filter, pageNumber, pageSize);
                if (result == null || result.Data == null || !result.Data.Any())
                    return NotFound(new ApiResponse<object>("No payments found matching the filter criteria.", 404));

                return Ok(new ApiResponse<PaginationResult<List<PaymentDTOResponse>>>(
                    "Filtered payments retrieved successfully.",
                    result,
                    200
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering payments");
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
