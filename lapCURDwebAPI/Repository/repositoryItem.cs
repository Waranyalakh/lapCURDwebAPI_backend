using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace lapCURDwebAPI.Repository
{
    public class repositoryItem
    {
        private readonly DataContext _dataContext;
        public repositoryItem(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        //-------------------- GetAll ----------------------------//
        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _dataContext.Items.ToListAsync();
        }
        //---------------Getby ID--------------------------------//
        public async Task<Item> GetItemsAsync(int Id) 
        {
            return await _dataContext.Items.FindAsync(Id);
        }

        //--------------------- Post --------------------------//
        public async Task<Item> AddItemAsync(Item items) 
        {
            _dataContext.Items.Add(items);
            await _dataContext.SaveChangesAsync();
            return items;
        }

        //------------------ put -------------------------------//
        public async Task UpdateAsync(Item updateItems) 
        {
            _dataContext.Items.Update(updateItems);
            await _dataContext.SaveChangesAsync();
        }


        //------------------- Delete ----------------------------//

        public async Task DeleteAsync(Item updateItem) 
        {
            _dataContext.Items.Remove(updateItem);
            await _dataContext.SaveChangesAsync();
        }

    }
}
