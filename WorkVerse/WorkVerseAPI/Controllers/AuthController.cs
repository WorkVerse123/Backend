using Application.DTOs.Email;
using Application.DTOs.Request;
using Application.Helper;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WorkVerseAPI.Models;
using static Application.DTOs.Email.OtpCache;

namespace WorkVerseAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IEmployeeProfileServices _employeeProfileServices;
		private readonly IEmployerProfileService _employerProfileServices;
		private readonly IJWTService _jwtService;
		private readonly IEmailService _emailService;

		public AuthController(IAuthService authService, IJWTService jwtService, IEmployeeProfileServices employeeProfileServices, IEmployerProfileService employerProfileServices, IEmailService emailService)
		{
			_authService = authService;
			_jwtService = jwtService;
			_employeeProfileServices = employeeProfileServices;
			_employerProfileServices = employerProfileServices;
			_emailService = emailService;
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
			if (user.RoleId == 3) // Employer
			{
				var employerId = await _employerProfileServices.GetEmployerIdByUserIdAsync(user.UserId);
				if (employerId == null)
				{
					return NotFound(new ApiResponse<object>("Employer profile not found.", 404));
				}
				user.EmployerId = (int)employerId;
			}
			else if (user.RoleId == 4) // Employee
			{
				var employeeId = await _employeeProfileServices.GetEmployeeIdByUserIdAsync(user.UserId);
				if (employeeId == null)
				{
					return NotFound(new ApiResponse<object>("Employee profile not found.", 404));
				}
				user.EmployeeId = (int)employeeId;
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

		[HttpPost("otp/send")]
		[AllowAnonymous]
		public async Task<IActionResult> SendOtp([FromBody] SendEmailDTORequest req)
		{
			if (string.IsNullOrWhiteSpace(req.Email))
				return BadRequest(new ApiResponse<object>("Email không được để trống.", 400));

			if (string.IsNullOrWhiteSpace(req.Purpose))
				return BadRequest(new ApiResponse<object>("Purpose không được để trống.", 400));

			// Chuẩn hóa Purpose để tránh lỗi client truyền sai kiểu
			var allowedPurposes = new[] { "AccountVerification", "PasswordReset", "ChangeEmail" };
			if (!allowedPurposes.Contains(req.Purpose))
				return BadRequest(new ApiResponse<object>("Purpose không hợp lệ.", 400));

			var otp = new Random().Next(100000, 999999).ToString();
			var result = OtpCache.SaveOtp(req.Email, otp, TimeSpan.FromMinutes(5), req.Purpose);

			if (!result.Success)
				return BadRequest(new ApiResponse<object>($"{result.ErrorMessage}", 400));

			string subject, body;
            switch (req.Purpose)
            {
                case "AccountVerification":
                    subject = "[WorkVerse] Xác thực tài khoản của bạn";
                    body = $@"
			<p>Xin chào,</p>
			<p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>WorkVerse</strong> — nền tảng việc làm thông minh dành cho sinh viên part-time và doanh nghiệp SME.</p>
			<p>Mã OTP xác thực tài khoản của bạn là:</p>
			<h2 style='color:#2d89ef; letter-spacing:3px'>{otp}</h2>
			<p>Mã này có hiệu lực trong vòng <strong>5 phút</strong>. Vui lòng không chia sẻ mã này với bất kỳ ai.</p>
			<hr>
			<p><strong>Thông tin bảo mật:</strong><br>
			WorkVerse sẽ không bao giờ yêu cầu bạn cung cấp mã OTP qua điện thoại hoặc email khác. 
			Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.</p>
			<p>Trân trọng,<br>Đội ngũ <strong>WorkVerse</strong></p>
		";
                    break;

                case "PasswordReset":
                    subject = "[WorkVerse] Mã OTP đặt lại mật khẩu của bạn";
                    body = $@"
			<p>Xin chào,</p>
			<p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản WorkVerse của bạn.</p>
			<p>Mã OTP để đặt lại mật khẩu là:</p>
			<h2 style='color:#e67e22; letter-spacing:3px'>{otp}</h2>
			<p>Mã này chỉ có hiệu lực trong <strong>5 phút</strong>. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
			<hr>
			<p><strong>Lưu ý bảo mật:</strong><br>
			Không chia sẻ mã này cho bất kỳ ai. Nếu nghi ngờ tài khoản bị truy cập trái phép, vui lòng đổi mật khẩu ngay sau khi đăng nhập.</p>
			<p>Thân mến,<br>Đội ngũ <strong>WorkVerse</strong></p>
		";
                    break;

                case "ChangeEmail":
                    subject = "[WorkVerse] Xác nhận thay đổi địa chỉ email";
                    body = $@"
			<p>Xin chào,</p>
			<p>Bạn đang thực hiện thao tác thay đổi địa chỉ email trong hệ thống WorkVerse.</p>
			<p>Mã OTP xác nhận thay đổi email là:</p>
			<h2 style='color:#27ae60; letter-spacing:3px'>{otp}</h2>
			<p>Mã này có hiệu lực trong vòng <strong>5 phút</strong>.</p>
			<hr>
			<p><strong>Cảnh báo bảo mật:</strong><br>
			Nếu bạn không yêu cầu thay đổi email, vui lòng không cung cấp mã này cho bất kỳ ai và liên hệ ngay với bộ phận hỗ trợ WorkVerse.</p>
			<p>Trân trọng,<br>Đội ngũ <strong>WorkVerse</strong></p>
		";
                    break;

                default:
                    return BadRequest(new ApiResponse<object>("Purpose không hợp lệ.", 400));
            }


            await _emailService.SendEmailAsync(req.Email, subject, body);
			return Ok(new ApiResponse<object>("Nếu email tồn tại, mã OTP đã được gửi.", 200));
		}


		[HttpPost("otp/verify")]
		[AllowAnonymous]
		public IActionResult VerifyOtp([FromBody] VerifyOtpDTORequest req)
		{
			if (string.IsNullOrWhiteSpace(req.Email) ||
				string.IsNullOrWhiteSpace(req.OtpCode) ||
				string.IsNullOrWhiteSpace(req.Purpose))
			{
				return BadRequest(new ApiResponse<object>("Email, OTP và Purpose không được để trống.", 400));
			}

			var result = OtpCache.VerifyOtp(req.Email, req.OtpCode, req.Purpose);

			return result switch
			{
				OtpVerifyResult.Success => Ok(new ApiResponse<object>("Xác thực thành công.", 200)),
				OtpVerifyResult.Expired => BadRequest(new ApiResponse<object>("Mã OTP đã hết hạn.", 400)),
				OtpVerifyResult.Locked => BadRequest(new ApiResponse<object>("Bạn đã nhập sai quá nhiều lần. Vui lòng thử lại sau 5 phút.", 403)),
				_ => BadRequest(new ApiResponse<object>("Mã OTP không đúng hoặc thao tác không hợp lệ.", 400))
			};
		}


	}
}
