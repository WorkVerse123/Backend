using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EmployerTypeController : ControllerBase
    {
        private readonly IEmployerTypeSevice _employerTypeSevice;
        public EmployerTypeController(IEmployerTypeSevice employerTypeSevice)
        {
            _employerTypeSevice = employerTypeSevice;
        }
        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployerTypes()
        {
            try
            {
                var employerTypes = await _employerTypeSevice.GetAllEmployerTypesAsync();
                return Ok(new ApiResponse<object>("Employer type get successfully.", employerTypes, 200));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
