using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using lapCURDwebAPI.Model;
using lapCURDwebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace lapCURDwebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly DataContext _dbContext;

        public AuthController(TokenService tokenService, DataContext dbContext)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginUser)
        {
            // ดึงข้อมูลผู้ใช้จากฐานข้อมูลโดยใช้ชื่อผู้ใช้ที่ส่งมา
            var user = _dbContext.Users
                .FirstOrDefault(u => u.UserName == loginUser.UserName);

            if (user == null)
            {
                return Unauthorized(); // ไม่พบผู้ใช้
            }

            // ตรวจสอบรหัสผ่านที่ให้มากับรหัสผ่านที่เก็บในฐานข้อมูล
            if (PasswordHelper.VerifyPassword(loginUser.PassWord, user.PassWordHash))
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized(); // รหัสผ่านไม่ถูกต้อง
        }
       

    }
}
