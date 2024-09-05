using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using lapCURDwebAPI.Model;
using lapCURDwebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lapCURDwebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly DataContext _dbContext;  // Inject DbContext ของคุณ

        public AuthController(TokenService tokenService, DataContext dbContext)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // ดึงข้อมูลผู้ใช้จากฐานข้อมูลโดยใช้ชื่อผู้ใช้ที่ส่งมา
            var user = _dbContext.Users
                .SingleOrDefault(u => u.UserName == loginRequest.UserName);

            if (user == null)
            {
                return Unauthorized(); // ไม่พบผู้ใช้
            }

            // ตรวจสอบรหัสผ่านที่ให้มากับรหัสผ่านที่เก็บในฐานข้อมูล
            if (BCrypt.Net.BCrypt.Verify(loginRequest.PassWord, user.PassWordHash))
            {
                var token = _tokenService.GenerateToken(loginRequest);
                return Ok(new { Token = token });
            }

            return Unauthorized(); // รหัสผ่านไม่ถูกต้อง
        }
    }
}
