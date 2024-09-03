using lapCURDwebAPI.Data;
using lapCURDwebAPI.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace lapCURDwebAPI.Repository
{
    public class repositoryUser
    {
        private readonly DataContext _dataContext;
        public repositoryUser(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //-------------------- GetAll ----------------------------//
        public async Task<List<User>> GetAllUsersAsync() 
        {
            return await _dataContext.Users.ToListAsync();
        }
        //---------------Getby ID---------------------------//

        public async Task<User> GetUserAsync(int Id) 
        {
            return await _dataContext.Users.FindAsync(Id);
        }

        //--------------------- Post --------------------------//

        public async Task<User> AddUserAsynce(User users) 
        {
            _dataContext.Users.Add(users);
            await _dataContext.SaveChangesAsync();
            return users;
        }
        //------------------ put -------------------------------//
        public async Task UpdateAsync(User updateuser) 
        {

            _dataContext.Users.Update(updateuser);
            await _dataContext.SaveChangesAsync();
            
        }

        //------------------- Delete ----------------------------//
        public async Task DeleteAsync(User user) 
        {
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
        }



        
    }
}
