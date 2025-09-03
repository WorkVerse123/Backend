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

        // POST /employees/{id}/applications
        [HttpPost("employees/{id}/applications")]
        public async Task<IActionResult> CreateApplication([FromRoute] int id, [FromBody] ApplicationDTORequest request)
        {
            try
            {
                var (isValid, error) = ApplicationValidationHelper.ValidateApplicationPostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));

                var result = await _applicationService.CreateApplicationAsync(id, request);
                return Ok(new ApiResponse<ApplicationItemDTO>("Application created successfully", result, 201));
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
    }

}
