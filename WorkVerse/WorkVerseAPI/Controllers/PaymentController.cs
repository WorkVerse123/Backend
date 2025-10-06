using Application.DTOs.Request;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Graders;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly PayOSService _payService;

        public PaymentController(PayOSService payService)
        {
            _payService = payService;
        }

        // POST /payments
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] SubscriptionPlanDTORequest plan)
        {
            try
            {
                if (plan == null || plan.PlanId <= 0 || plan.Price <= 0)
                    return BadRequest(new ApiResponse<object>("Invalid subscription plan data.", 400));

                var checkoutUrl = await _payService.CreatePaymentLink(plan);
                if (string.IsNullOrEmpty(checkoutUrl))
                    return StatusCode(500, new ApiResponse<object>("Failed to create payment link.", 500));

                return Ok(new ApiResponse<object>("Payment link created successfully.", new { checkoutUrl }, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /payments/webhook
        [HttpPost("webhook")]
        public IActionResult Webhook([FromBody] dynamic payload)
        {
            try
            {
                // TODO: Validate signature and update DB as needed
                Console.WriteLine("Webhook data: " + payload?.ToString());
                return Ok(new ApiResponse<object>("Webhook received successfully.", null, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /payments/success
        [HttpGet("success")]
        public IActionResult PaymentSuccess([FromQuery] string orderCode, [FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderCode) || string.IsNullOrWhiteSpace(status))
                    return BadRequest(new ApiResponse<object>("Order code and status are required.", 400));

                return Ok(new ApiResponse<object>(
                    "Payment successful.",
                    new { orderCode, status },
                    200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /payments/cancel
        [HttpGet("cancel")]
        public IActionResult PaymentCancel()
        {
            try
            {
                return Ok(new ApiResponse<object>("Payment was cancelled.", null, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
