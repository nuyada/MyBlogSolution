using ASP.Blog.MVC.ViewModels.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface IRoleService
    {
        public RoleViewModel AddRole();
        public Task AddRole(RoleViewModel model);
        public List<RoleViewModel> AllRoles();
        public Task<RoleViewModel> UpdateRole(string roleId);
        public Task UpdateRole(RoleViewModel model);
        public Task DeleteRole(string roleId);
    }
}
