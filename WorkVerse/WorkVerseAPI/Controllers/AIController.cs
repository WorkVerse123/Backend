using Application.DTOs.Request;
using Application.Interfaces.IServicies;
using Application.Servicies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Application.DTOs.Response;


namespace WorkVerseAPI.Controllers
{
    [Route("api/chatbot-ai")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly IEmployerProfileService _employerProfileService;
        private readonly IJobService _jobService;
        private readonly IEmployeeProfileServices _employeeProfileService;
        public AIController(AIService aiService, IEmployerProfileService employerProfileService, IJobService jobService, IEmployeeProfileServices employeeProfileService)
        {
            _aiService = aiService;
            _employerProfileService = employerProfileService;
            _jobService = jobService;
            _employeeProfileService = employeeProfileService;
        }

        [HttpPost("employee")]
        public async Task<IActionResult> ChatBotAIEmployee([FromBody] QueryAIDTORequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Query))
                return BadRequest(new { error = "Query is required." });

            var rawJson = await _aiService.ExtractPromptEmployeeAsync(req.Query);

            // 1. Cleanup backtick ```json ... ``` nếu có
            var cleaned = rawJson
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            try
            {
                AIQueryResult result = null;

                // 2. Nếu JSON bắt đầu bằng '{' => deserialize trực tiếp
                if (cleaned.TrimStart().StartsWith("{"))
                {
                    result = JsonSerializer.Deserialize<AIQueryResult>(cleaned, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    // 3. Nếu AI trả wrapper dạng {"raw": "..."} => deserialize wrapper trước
                    var wrapper = JsonSerializer.Deserialize<Dictionary<string, string>>(cleaned);
                    if (wrapper != null && wrapper.TryGetValue("raw", out var innerJson))
                    {
                        result = JsonSerializer.Deserialize<AIQueryResult>(innerJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    else
                    {
                        return BadRequest(new { error = "Invalid AI response format." });
                    }
                }

                if (result == null)
                    return BadRequest(new { error = "AI response could not be parsed." });

                // --- Trường hợp không hợp lệ ---
                if (result.Employee != null)
                {
                    return BadRequest(new { Message = "Bạn chỉ được search công việc/nhà tuyển dụng." });
                }

                // --- Nếu search Job + Employer ---
                if (result.Job != null && result.Employer != null)
                {
                    var jobWithEmployers = await _jobService.SearchJobByEmployerAIResult(result.Job, result.Employer);
                    return Ok(jobWithEmployers ?? Enumerable.Empty<JobWithEmployerAIDTOResponse>());
                }

                // --- Nếu chỉ search Job ---
                if (result.Job != null)
                {
                    var jobs = await _jobService.SearchJobByAIResult(result.Job);
                    return Ok(jobs ?? Enumerable.Empty<JobAIDTOResponse>());
                }

                // --- Nếu chỉ search Employer ---
                if (result.Employer != null)
                {
                    var employers = await _employerProfileService.SearchEmployerByAIResult(result.Employer);
                    return Ok(employers ?? Enumerable.Empty<EmployerAIDTOResponse>());
                }



                // --- Không liên quan ---
                return NotFound(new { Message = "Câu hỏi không liên quan đến dữ liệu tuyển dụng." });
            }
            catch (Exception ex)
            {
                // Trả raw JSON và lỗi chi tiết để debug
                return Ok(new { raw = cleaned, error = ex.Message });
            }
        }

        [HttpPost("employer")]
        public async Task<IActionResult> ChatBotAIEmployer([FromBody] QueryAIDTORequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Query))
                return BadRequest(new { error = "Query is required." });

            var rawJson = await _aiService.ExtractPromptEmployerAsync(req.Query);

            // 1. Cleanup backtick ```json ... ``` nếu có
            var cleaned = rawJson
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            try
            {
                AIQueryResult result = null;

                // 2. Nếu JSON bắt đầu bằng '{' => deserialize trực tiếp
                if (cleaned.TrimStart().StartsWith("{"))
                {
                    result = JsonSerializer.Deserialize<AIQueryResult>(cleaned, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    // 3. Nếu AI trả wrapper dạng {"raw": "..."} => deserialize wrapper trước
                    var wrapper = JsonSerializer.Deserialize<Dictionary<string, string>>(cleaned);
                    if (wrapper != null && wrapper.TryGetValue("raw", out var innerJson))
                    {
                        result = JsonSerializer.Deserialize<AIQueryResult>(innerJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    else
                    {
                        return BadRequest(new { error = "Invalid AI response format." });
                    }
                }

                if (result == null)
                    return BadRequest(new { error = "AI response could not be parsed." });

                // --- Trường hợp không hợp lệ ---
                if (result.Job != null || result.Employer != null)
                {
                    return BadRequest(new { Message = "Bạn chỉ được search ứng viên riêng." });
                }

                if (result.Employee != null)
                {
                    var employees = await _employeeProfileService.SearchEmployeeByAIResult(result.Employee);
                    return Ok(employees ?? Enumerable.Empty<EmployeeAIDTOResponse>());
                }

                // --- Không liên quan ---
                return NotFound(new { Message = "Câu hỏi không liên quan đến dữ liệu tuyển dụng." });
            }
            catch (Exception ex)
            {
                // Trả raw JSON và lỗi chi tiết để debug
                return Ok(new { raw = cleaned, error = ex.Message });
            }
        }
    }

}
