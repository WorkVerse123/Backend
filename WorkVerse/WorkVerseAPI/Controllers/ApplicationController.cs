using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{

    [ApiController]
    [Route("applications")]
    public class ApplicationController : ControllerBase
    {
        // This is a placeholder for the actual implementation of user service
        private readonly IEmployeeProfileServices _employeeProfileService;
        private readonly IBusyTimeService _busyTimeService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IApplicationService _applicationService;
        public ApplicationController(IEmployeeProfileServices employeeProfileService, IBusyTimeService busyTimeService, IBookmarkService bookmarkService, IApplicationService applicationService)
        {
            _employeeProfileService = employeeProfileService;
            _busyTimeService = busyTimeService;
            _bookmarkService = bookmarkService;
            _applicationService = applicationService;
        }


        // PUT /applications/{id}/withdrawn

        [HttpPut("{id}/withdrawn")]
        public async Task<IActionResult> UpdateApplicationWithdrawn([FromRoute] int id)
        {
            try
            {
                var result = await _applicationService.UpdateApplicationWithdrawnAsync(id);
                return Ok(new ApiResponse<ApplicationItemDTO>("Application updated successfully", result, 200));
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
        public async Task<IActionResult> GetApplicationDetailById([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse<object>("Application ID must be greater than 0", 400));
                }

                var result = await _applicationService.GetApplicationDetailByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>($"No application found with ID {id}", 404));
                }

                return Ok(new ApiResponse<ApplicationItemDetailDTO>("Get applications detail successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /employers/{id}/job/{job_id}/applications
        [HttpGet("/employers/{id}/job/{job_id}/applications")]
        public async Task<IActionResult> GetAllJobApplication([FromRoute] int id, [FromRoute] int job_id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _applicationService.GetJobApplicationsAsync(id,job_id,pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Job Applications found", 404));
                }

                return Ok(new ApiResponse<JobApplicationsResponseDTO>("Registration successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET/stats
        [HttpGet("/stats")]
        public async Task<IActionResult> GetStatInformation()
        {
            try
            {
                var result = await _applicationService.GetStatsInformationAsync();
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"Get stats information failed", 404));
                }

                return Ok(new ApiResponse<StatsInformationDTOResponse>("Get stats information successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }

}
