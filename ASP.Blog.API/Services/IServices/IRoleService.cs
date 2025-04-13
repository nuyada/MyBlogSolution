using ASP.Blog.MVC.ViewModels.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface IRoleService
    {
        public RoleRequest AddRole();
        public Task AddRole(RoleAddRequest model);
        public List<RoleRequest> AllRoles();
        public Task<RoleRequest> UpdateRole(string roleId);
        public Task UpdateRole(RoleRequest model);
        public Task DeleteRole(string roleId);
    }
}
