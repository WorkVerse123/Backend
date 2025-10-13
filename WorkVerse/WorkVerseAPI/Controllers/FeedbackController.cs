using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helper;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/feedbacks")]
    [Authorize]

    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
           _feedbackService = feedbackService;
        }

        // POST /feedbacks
        [HttpPost]
        [Authorize(Roles = "3,4")]

        public async Task<IActionResult> CreateFeedback([FromBody] SendFeedbackDTORequest request)
        {
            try
            {
                var (isValid, error) = FeedbackValidationHelper.ValidationSendFeedbackRequest(request);
                if (!isValid)
                    return BadRequest(new ApiResponse<object>(error, 400));
                var result = await _feedbackService.CreateFeedbackAsync(request);
                return Ok(new ApiResponse<FeedbackItemDTOResponse>("Feedback created successfully", result, 201));
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(400, new ApiResponse<object>(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /feedbacks
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllFeedbacks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _feedbackService.GetFeedbackListAsync(pageNumber, pageSize);
                if (result == null)
                {
                    return NotFound(new ApiResponse<object>(
                        "Something went wrong. Please try again later.", 404));
                }

                return Ok(new ApiResponse<FeedbackListDTOResponse>("Get list of feedbacks successfully", result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // PUT /feedbacks/{id}/handle
        [HttpPut("{id}/handle")]
        [Authorize(Roles = "1,2")]

        public async Task<IActionResult> UpdateFeedbackHandler([FromRoute] int id, [FromBody] UpdateFeedbackHandlerDTORequest request)
        {
            try
            {
                var updated = await _feedbackService.UpdateFeedbackHandleAsync(id, request);
                if (!updated)
                {
                    return StatusCode(500, new ApiResponse<object>("Update feedback failed", 500));
                }

                return Ok(new ApiResponse<object>("Feedback updated successfully.", null, 200));
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
