using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using lapCURDwebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lapCURDwebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly repositoryUser _repositoryUsers;

        public UserController(repositoryUser repositoryUsers)
        {
            _repositoryUsers = repositoryUsers;
        }

        //---Get all---//
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            try
            {
                var users = await _repositoryUsers.GetAllUsersAsync();
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //--- GET ID ---//
        [HttpGet("{Id}")]
        public async Task<ActionResult<User>> GetUser(int Id)
        {
            try
            {
                var user = await _repositoryUsers.GetUserAsync(Id);
                if (user == null)
                {
                    return NotFound("Data not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //--- Post ---//
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            try
            {
                // แฮชรหัสผ่านก่อนที่จะบันทึกลงในฐานข้อมูล
                user.PassWordHash = PasswordHelper.HashPassword(user.PassWordHash);

                var addedUser = await _repositoryUsers.AddUserAsynce(user);
                return CreatedAtAction(nameof(GetUser), new { Id = addedUser.Id }, addedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //--- Put ---//
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User updateUser)
        {
            if (id != updateUser.Id)
            {
                return BadRequest("ID mismatch.");
            }

            try
            {
                var dbUser = await _repositoryUsers.GetUserAsync(updateUser.Id);
                if (dbUser == null)
                {
                    return NotFound("Data not found.");
                }

                dbUser.Name = updateUser.Name;
                // อัปเดตการแฮชรหัสผ่านถ้ามีการเปลี่ยนแปลง
                if (!string.IsNullOrEmpty(updateUser.PassWordHash))
                {
                    dbUser.PassWordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.PassWordHash);
                }
                await _repositoryUsers.UpdateAsync(dbUser);

                return Ok(dbUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //--- Delete ---//
        [HttpDelete("{Id}")]
        public async Task<ActionResult<User>> DeleteUser(int Id)
        {
            try
            {
                var dbUser = await _repositoryUsers.GetUserAsync(Id);
                if (dbUser == null)
                {
                    return NotFound("Data not found.");
                }

                await _repositoryUsers.DeleteAsync(dbUser);
                return Ok(dbUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
