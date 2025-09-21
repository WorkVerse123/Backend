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

            var user = await _authService.ValidateUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new ApiResponse<object>("Invalid account or password.", 401));
            }
            user.IsPremium = await _authService.IsPremiumAsync(user.UserId);
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
            if (!existsRole)
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
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDTORequest request)
        {
            var (isValid, error) = UserValidationHelper.ValidateChangePassword(request);
            var user = await _authService.ValidateUserAsync(request.Email, request.CurrentPassword);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<object>("Old password is incorrect.", 401));
            }
            var isUpdated = await _authService.UpdatePasswordAsync(request);
            if (!isUpdated)
            {
                return BadRequest(new ApiResponse<object>("Failed to update password.", 400));
            }
            return Ok(new ApiResponse<object>("Password updated successfully.", 200));
        }
    }
}
