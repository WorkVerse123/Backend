using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // POST /reports
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] SubmitReportDTORequest request)
        {
            try
            {
                var (isValid, error) = ReportValidationHelper.ValidateSubmitReport(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _reportService.CreateReportAsync(request);
                return Ok(new ApiResponse<ReportDetailsDTOResponse>("Report created successfully", result, 201));
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

        // GET /reports
        [HttpGet]
        public async Task<IActionResult> GetAllCandidates([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _reportService.GetReportListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<ReportListDTOResponse>("Get list of reports successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
        // PUT /reports/{id}/status
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReportStatus([FromRoute] int id, [FromBody] UpdateReportStatusDTORequest request)
        {
            try
            {
                var updated = await _reportService.UpdateReportStatusAsync(id, request);
                if (!updated)
                {
                    return StatusCode(500, new ApiResponse<object>("Update report failed", 500));
                }

                return Ok(new ApiResponse<object>("Report updated successfully.", null, 200));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 404));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
