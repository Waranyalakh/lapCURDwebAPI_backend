using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using lapCURDwebAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace lapCURDwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserItemController(repositoryUserItem repositoryUserItems) : ControllerBase
    {

        //----- GET ALL ---//
        [HttpGet]
        public async Task<List<Useritem>> GetAllUserItem()
        {
            var usersItem = await repositoryUserItems.GetAllUserItemAsync();
            return usersItem;
        }
        //--------------- GET ID --------------------------------------//

        [HttpGet("{Id}")]
        public async Task<ActionResult<Useritem>> GetUser(int Id)
        {
            var usersItem = await repositoryUserItems.GetUserItemAsync(Id);
            if (usersItem == null)
            {
                return BadRequest("Data not found");

            }
            return Ok(usersItem);

        }
        //---------------Post-----------------------------------------//

        [HttpPost]
        public async Task<ActionResult<Useritem>> AddUser(Useritem usersItem)
        {
            var addedUser = await repositoryUserItems.AddUserAsynce(usersItem);
            return Ok(addedUser);
        }
        //-------------- Put ----------------------------------------//
        [HttpPut]
        public async Task<ActionResult<Useritem>> UpdateUser(Useritem updateusersItem)
        {
            var dbUserItem = await repositoryUserItems.GetUserItemAsync(updateusersItem.Id);
            if (dbUserItem == null)
                return BadRequest("data not found.");

            dbUserItem.Id = updateusersItem.Id; //ถ้าไม่ใส่ตัวนี้ไป ข้อมูลจะไม่อัพเดท
            await repositoryUserItems.UpdateAsync(dbUserItem);

            return Ok(dbUserItem);
        }
        //------------ Delete -----------------------------------//

        [HttpDelete]
        public async Task<ActionResult<Useritem>> DeleteUser(User updateUser)
        {
            var dbUserItem = await repositoryUserItems.GetUserItemAsync(updateUser.Id);
            if (dbUserItem == null)
                return BadRequest("data not found.");

            await repositoryUserItems.DeleteAsync(dbUserItem);//โค้ดนี้คืออะไรเอ่ย??น่าจะจำเป็น พอไม่มีบรรทัดนี้ โค้ดไม่สามารถ ลบ หรือ อัพเดท ได้ด้วย
            return Ok(dbUserItem);
        }
    }
}
