using Application.DTOs.Response;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("companies")]
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
                var result = await _employerProfileService.GetAllCompaniesAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        $"No Company found", 404));
                }

                return Ok(new ApiResponse<EmployerProfileDTOResponse>("Get companies information successful", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
