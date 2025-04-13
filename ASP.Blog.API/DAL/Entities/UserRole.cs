using Microsoft.AspNetCore.Identity;

namespace ASP.Blog.MVC.DAL.Entities
{
    public class UserRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
