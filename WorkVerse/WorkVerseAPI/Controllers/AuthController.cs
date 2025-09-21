using Application.DTOs.Request;
using Application.Helper;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;

namespace WorkVerseAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJWTService _jwtService;

        public AuthController(IAuthService authService, IJWTService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTORequest request)
        {
            var (isValid, error) = UserValidationHelper.ValidateLogin(request);
            if (!isValid)
                return BadRequest(new ApiResponse<object>(error, 400));
            string account = request.Email;
            var user = await _authService.ValidateUserAsync(account, request.Password);

            if (user == null)
            {
                return Unauthorized(new ApiResponse<object>("Invalid account or password.", 401));
            }

            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new ApiResponse<string>("Login successful.", token, 200));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTORequest request)
        {
            var (isValid, error) = UserValidationHelper.ValidateRegister(request);
            if (!isValid)
                return BadRequest(new ApiResponse<object>(error, 400));
            string account = request.Email ?? request.PhoneNumber;

            var existsUser = await _authService.ExsitedUser(request.Email, request.PhoneNumber);
            if (existsUser)
            {
                return Conflict(new ApiResponse<object>("Email already exists.", 409));
            }
            var existsRole = await _authService.ExsitedRole(request.RoleId);
            if (existsRole)
            {
                return BadRequest(new ApiResponse<object>("RoleId not found.", 400));
            }

            var newUser = await _authService.CreatedAccountAsync(request);
            if (newUser == null)
            {
                return BadRequest(new ApiResponse<object>("Failed to create account.", 400));
            }

            return CreatedAtAction(nameof(Register), new { id = newUser.UserId }, new ApiResponse<object>("Account created successfully.", newUser, 201));
        }
    }
}
