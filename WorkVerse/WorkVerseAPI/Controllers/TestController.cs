using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WorkVerseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly WorkVerseDBContext _context;

        public TestController(WorkVerseDBContext context)
        {
            _context = context;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Thử truy vấn đơn giản
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                    return Ok("Kết nối SQL thành công!");
                else
                    return StatusCode(500, "Không thể kết nối SQL");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi kết nối: {ex.Message}");
            }
        }
    }
}
