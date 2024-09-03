using lapCURDwebAPI.Model;
using lapCURDwebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace lapCURDwebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // ตรวจสอบชื่อผู้ใช้และรหัสผ่าน (ในกรณีจริงควรตรวจสอบจากฐานข้อมูล)
            if (loginRequest.UserName == "testuser" && loginRequest.PassWord == "password")
            {
                var token = _tokenService.GenerateToken(loginRequest);
                return Ok(new { Token = token });
            }

            return Unauthorized(); // Uanauthorized is reutrn satetus 401
        }
    }
}
