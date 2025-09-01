using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.IServicies;
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

        public EmployeeController(IEmployeeProfileServices employeeProfileService)
        {
            _employeeProfileService = employeeProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] EmployeeProfileDTOPostRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse<object>("Invalid request",
                        new List<string> { "Profile request cannot be null" }, 400));
                }
                var check = await _employeeProfileService.GetByUserIdAsync(request.UserId);
                if (check != null)
                {
                    return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile founded",check,200
                    ));
                }
                var result = await _employeeProfileService.CreateProfileAsync(request);
                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile created successfully", result, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("Internal server error", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {           
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Invalid employee ID",
                        new List<string> { "Employee ID must be greater than 0" }, 400));
                }

                var result = await _employeeProfileService.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>("Profile not found",
                        new List<string> { $"No profile found with ID {id}" }, 404));
                }

                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile retrieved successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("Internal server error", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] EmployeeProfileDTOPutRequest request, [FromRoute] int id)
        {
            try
            {
                if (request == null || id == null)
                {
                    return BadRequest(new ApiResponse<object>("Invalid request",
                        new List<string> { "Request body is null or EmployeeId mismatch" }, 400));
                }

                var result = await _employeeProfileService.UpdateProfileAsync(id,request);
                return Ok(new ApiResponse<EmployeeProfileDTOResponse>("Profile updated successfully", result, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>("Profile not found", new List<string> { ex.Message }, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("Internal server error", new List<string> { ex.Message }, 500));
            }
        }

    }

}
