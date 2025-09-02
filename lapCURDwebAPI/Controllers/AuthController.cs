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
            try
            {
                // ดึงข้อมูลผู้ใช้จากฐานข้อมูลโดยใช้ชื่อผู้ใช้ที่ส่งมา
                var user = _dbContext.Users
                    .FirstOrDefault(u => u.UserName == loginUser.UserName);

                if (user == null)
                {
                    return BadRequest(new { message = "ไม่พบผู้ใช้" }); // ข้อมูลชื่อผู้ใช้ไม่ถูกต้อง
                }

                // ตรวจสอบรหัสผ่านที่ให้มากับรหัสผ่านที่เก็บในฐานข้อมูล
                if (!PasswordHelper.VerifyPassword(loginUser.PassWord, user.PassWordHash))
                {
                    return BadRequest(new { message = "รหัสผ่านไม่ถูกต้อง" }); // รหัสผ่านไม่ตรงกับที่เก็บ
                }

                // สร้าง token เมื่อข้อมูลถูกต้อง
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token }); // ส่งกลับ token

            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดที่ไม่คาดคิด
                return StatusCode(500, new { message = "เกิดข้อผิดพลาดภายในระบบ", error = ex.Message });
            }
        }




    }
}
