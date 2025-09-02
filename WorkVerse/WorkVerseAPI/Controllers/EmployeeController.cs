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


        public EmployeeController(IEmployeeProfileServices employeeProfileService, IBusyTimeService busyTimeService)
        {
            _employeeProfileService = employeeProfileService;
            _busyTimeService = busyTimeService;
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
                var check = await _employeeProfileService.GetByUserIdAsync(userId);
                if (check != null)
                {
                    return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile founded",check,200
                    ));
                }
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
    }

}
