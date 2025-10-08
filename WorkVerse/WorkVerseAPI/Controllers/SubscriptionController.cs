using Application.DTOs.Response;
using Application.Interfaces.IServicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET /subscriptions/plans
        [HttpGet("plans")]
        public async Task<IActionResult> GetAllPlans()
        {
            try
            {
                var plans = await _subscriptionService.GetAll();
                return Ok(new ApiResponse<IEnumerable<SubscriptionPlanDTOResponse>>(
                    "Get list of subscription plans successfully", plans, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // GET /subscriptions/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserPlan([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse<object>("User ID must be greater than 0", 400));

                var plan = await _subscriptionService.GetByUser(id);
                if (plan == null)
                    return NotFound(new ApiResponse<object>("No subscription plan found for this user", 404));

                return Ok(new ApiResponse<SubscriptionPlanDTOResponse>(
                    "Get user's subscription plan successfully", plan, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }

        // POST /subscriptions/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSubscription([FromQuery] int userId, [FromQuery] int planId)
        {
            try
            {
                if (userId <= 0 || planId <= 0)
                    return BadRequest(new ApiResponse<object>("UserId and PlanId must be greater than 0", 400));

                var success = await _subscriptionService.RegisterSubsciption(userId, planId);
                if (!success)
                    return BadRequest(new ApiResponse<object>("User already subscribed to this plan.", 400));

                return Ok(new ApiResponse<object>("Subscription registered successfully.", null, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(ex.Message, 500));
            }
        }
    }
}
