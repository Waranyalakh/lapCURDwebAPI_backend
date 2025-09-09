using lapCURDwebAPI.Entity;
using lapCURDwebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lapCURDwebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(repositoryItem repositoryItems) : ControllerBase
    {

        //---Get all---//
        [HttpGet]
        public async Task<ActionResult<List<Item>>> GetAllItem()
        {

            var item = await repositoryItems.GetAllItemsAsync();
            return Ok(item);
        }
        //--------------- GET ID --------------------------------------//
        [HttpGet("{Id}")]
        public async Task<ActionResult<Item>> GetItem(int Id)
        {
            var items = await repositoryItems.GetItemsAsync(Id);
            if (items == null)
            {
                return BadRequest("Data not found.");
            }
            return Ok(items);
        }

        //--------------- Post ---------------------------------------//

        [HttpPost]
        public async Task<ActionResult<Item>> AddItems(Item items)
        {
            var addItem = await repositoryItems.AddItemAsync(items);
            return Ok(addItem);
        }

        //-------------- Put ----------------------------------------//

        [HttpPut("{id}")]

        public async Task<ActionResult<Item>> UpdateItem(int id, [FromBody] Item updateItems) 
        {
  
            var dbItem = await repositoryItems.GetItemsAsync(updateItems.Id);
            if (dbItem == null)
                return BadRequest("data not found.");

            dbItem.Name = updateItems.Name; //ถ้าไม่ใส่ตัวนี้ไป ข้อมูลจะไม่อัพเดท
            await repositoryItems.UpdateAsync(dbItem);

            return Ok(dbItem);

        }

        //------------ Delete -----------------------------------//
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var dbItem = await repositoryItems.GetItemsAsync(id);
            if (dbItem == null)
                return BadRequest("data not found.");

            await repositoryItems.DeleteAsync(dbItem);
            return Ok(dbItem);
        }

    }
}
