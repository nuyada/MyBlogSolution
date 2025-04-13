using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.ViewModels.Role;

namespace ASP.Blog.MVC.Extentions
{
    public static class RoleFromModel
    {
        public static UserRole Convert(this UserRole role, RoleRequest roleeditvm)
        {
            role.Name = roleeditvm.Name;
            role.Description = roleeditvm.Description;

            return role;
        }
    }
}
