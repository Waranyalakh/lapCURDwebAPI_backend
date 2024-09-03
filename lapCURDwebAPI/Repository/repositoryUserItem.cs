using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace lapCURDwebAPI.Repository
{
    public class repositoryUserItem
    {
        private readonly DataContext _dataContext;
        public repositoryUserItem(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //----------- GET ALL -----------------------//
        public async Task<List<Useritem>> GetAllUserItemAsync() 
        {
            return await _dataContext.Useritems.ToListAsync();
        }
        //---------------Getby ID---------------------------//

        public async Task<Useritem> GetUserItemAsync(int Id)
        {
            return await _dataContext.Useritems.FindAsync(Id);
        }
        //----------- POST ---------------------------//
        public async Task<Useritem> AddUserAsynce(Useritem usersItem)
        {
            _dataContext.Useritems.Add(usersItem);
            await _dataContext.SaveChangesAsync();
            return usersItem;
        }
        //----------- PUT --------------------------------//
        public async Task UpdateAsync(Useritem updateuseritem)
        {
            _dataContext.Useritems.Update(updateuseritem);
            await _dataContext.SaveChangesAsync();

        }
        //------------------- Delete ----------------------------//
        public async Task DeleteAsync(Useritem usersItem)
        {
            _dataContext.Useritems.Remove(usersItem);
            await _dataContext.SaveChangesAsync();
        }

    }
}
