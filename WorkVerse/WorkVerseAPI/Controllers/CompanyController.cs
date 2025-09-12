using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly IEmployeeProfileServices _employeeProfileService;
        private readonly IBusyTimeService _busyTimeService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IApplicationService _applicationService;
        private readonly IEmployerProfileService _employerProfileService;
        public CompanyController(IEmployeeProfileServices employeeProfileService, IBusyTimeService busyTimeService, IBookmarkService bookmarkService, IApplicationService applicationService, IEmployerProfileService employerProfileService)
        {
            _employeeProfileService = employeeProfileService;
            _busyTimeService = busyTimeService;
            _bookmarkService = bookmarkService;
            _applicationService = applicationService;
            _employerProfileService = employerProfileService;
        }

        // GET /companies
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies( [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _employerProfileService.GetAllEmployersAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Company found", 404));
                }

                return Ok(new ApiResponse<ListEmployerProfileDTOResponse>("Get companies information successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST/company-setup
        [HttpPost("company-setup")]
        public async Task<IActionResult> CreateEmployerProfile([FromBody] EmployerProfileDTORequest request)
        {
            try
            {
                var (isValid, error) = EmployerProfileValidationHelper.ValidateEmployerProfilePostRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _employerProfileService.CreateEmployerProfileAsync(request);
                return Ok(new ApiResponse<object>("Employer Profile created successfully", 201));
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
    }
}
