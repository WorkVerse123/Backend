using Application.DTOs.Request;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Graders;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IThirdPaymentService _payService;
        private readonly IPaymentService _payment;

        public PaymentController(IThirdPaymentService payService, IPaymentService payment)
        {
            _payService = payService;
            _payment = payment;
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
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync(); 

            try
            {
                //var data = _payService.VerifyWebhookData(body); 

                // hoặc data.desc
                // TODO: update DB theo trạng thái

                return Ok(new ApiResponse<object>("Webhook verified successfully.", null, 200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, 400));
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

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            try
            {
                var payment = _payment.GetById(id).Result;
                if (payment == null)
                    return NotFound(new ApiResponse<object>($"Payment with id {id} not found.", 404));
                return Ok(new ApiResponse<object>("Payment retrieved successfully.", payment, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
