using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{

    [ApiController]
    [Route("api/applications")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
           
            _applicationService = applicationService;
        }


        // PUT /applications/{id}/withdrawn

        [HttpPut("{id}/withdrawn")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> UpdateApplicationWithdrawn([FromRoute] int id)
        {
            try
            {
                var result = await _applicationService.WithdrawApplicationAsync(id);
                return Ok(new ApiResponse<JobApplicationItemDTO>("Application updated successfully", result, 200));
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

        // GET /applications/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "3,4")]
        public async Task<IActionResult> GetApplicationDetailById([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Application ID must be greater than 0", 400));
                }

                var result = await _applicationService.GetApplicationDetailsByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>($"No application found with ID {id}", 404));
                }

                return Ok(new ApiResponse<JobApplicationDetailsDTOResponse>("Get applications detail successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        

        // GET/stats
        [HttpGet("stats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatInformation()
        {
            try
            {
                var result = await _applicationService.GetPlatformStatsAsync();
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"Get stats information failed", 404));
                }

                return Ok(new ApiResponse<PlatformStatsResponseDTOResponse>("Get stats information successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /applications/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "3")]

        public async Task<IActionResult> UpdateApplicationStatus([FromRoute] int id, [FromBody] UpdateApplicationStatusDTORequest request)
        {
            try
            {
                var updated = await _applicationService.UpdateApplicationStatusAsync(id, request);
                if (!updated)
                {
                    return StatusCode(500, new ApiResponse<object>("Update application status failed", 500));
                }

                return Ok(new ApiResponse<object>("Application status updated successfully.", null, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 404));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 500));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }

}
