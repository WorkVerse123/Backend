using Application.DTOs.Request;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WorkVerseAPI.Controllers
{
    [Route("api/[controller]")]
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
            string  account = request.Email ?? request.PhoneNumber;
            var user = await _authService.ValidateUserAsync(account, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid account or password." });
            }
            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTORequest request)
        {
            string account = request.Email ?? request.PhoneNumber;
            var existingUser = await _authService.ValidateUserAsync(account, request.Password);
            if (existingUser != null)
            {
                return Conflict(new { message = "Account already exists." });
            }
            var newUser = await _authService.CreatedAccountAsync(request);
            if (newUser == null)
            {
                return BadRequest(new { message = "Failed to create account." });
            }
            return CreatedAtAction(nameof(Register), new { id = newUser.UserId }, newUser);
        }
    }
}
