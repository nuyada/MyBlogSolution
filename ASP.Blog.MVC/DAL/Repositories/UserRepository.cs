using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Blog.MVC.DAL.Repositories
{
    public class UserRepository : Repository<User>
    {
        BlogContext db;
        public UserRepository(BlogContext db) : base(db) { this.db = db; }

        public List<User> GetUsers() 
        {
            return Set.ToList();
        }
        public User GetUserById(string UserId)
        {
            return Set.Where(x => x.Id == UserId).FirstOrDefault();
        }
        public void AddUser(User user, UserRole userRole = null)
        {
            if (userRole != null)
            {
                var _user = user;

                if (db.Roles.Where(x => x.Name == "User").FirstOrDefault() == null)
                {
                    db.Roles.Add(new UserRole() { Name = "User", Description = "Ordinary User" });
                    db.SaveChanges();
                }
            }
            Set.Add(user); 
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }
        public void DeleteUser(User user)
        {
            Delete(user);
        }
    }
}
