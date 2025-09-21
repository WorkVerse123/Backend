using Application.DTOs.Request;
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
        private readonly IStaffProfileService _staffProfileService;
        private readonly IUserService _userService;
        public AdminController(ILogger<AdminController> logger, IStaffProfileService staffProfileService, IUserService userService)
        {
            _logger = logger;
            _staffProfileService = staffProfileService;
            _userService = userService;
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody] StaffProfileDTORequest staffProfile)
        {
            var result = await _staffProfileService.Create(staffProfile);

            if (result == null)
            {
                return BadRequest(new ApiResponse<object>("Failed to create staff profile.", 400));
            }

            return Ok(new ApiResponse<object>("Create staff profile success!",200));
        }

        [HttpPut("user/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UserUpdateStatusDTORequest userUpdate)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("User ID must be greater than 0", 400));
                }
                var result = await _userService.UpdateStatusAsync(id, userUpdate.NewStatus);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>($"User with ID {id} not found or status not updated.", 404));
                }
                return Ok(new ApiResponse<object>("User status updated successfully.", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for user id {UserId}", id);
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
