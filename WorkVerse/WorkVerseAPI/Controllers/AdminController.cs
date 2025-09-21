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
        public AdminController(ILogger<AdminController> logger, IStaffProfileService staffProfileService)
        {
            _logger = logger;
            _staffProfileService = staffProfileService;
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
    }
}
