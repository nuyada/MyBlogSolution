using ASP.Blog.MVC.Controllers;
using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels.Role;
using ASP.Blog.MVC.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Services
{
    public class RoleService : IRoleService
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public RoleViewModel AddRole()
        {
            _logger.LogInformation("Выполняется переход на страницу добавления роли.");
            return new RoleViewModel();
        }

        public async Task AddRole(RoleViewModel model)
        {
            //Инициализируем так, чтобы заполнить ID
            var role = new UserRole();
            var roleData = _mapper.Map<UserRole>(model);
            //var roleData = _mapper.Map<UserRole>(model);
            role.Name = roleData.Name;
            role.Description = roleData.Description;

            await _roleManager.CreateAsync(role);
            _logger.LogInformation($"Создана роль {role.Name}");
        }

        public List<RoleViewModel> AllRoles()
        {
            var repo = _unitOfWork.GetRepository<UserRole>() as UserRoleRepository;
            var roles = repo.GetUserRoles();
            var rolesView = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleViewModel>(role));
            }
            _logger.LogInformation($"Просмотр всех ролей");

            return rolesView;
        }

        public async Task DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                _logger.LogInformation($"Роль с ID = {roleId} удалена.");
            }
        }

        public async Task<RoleViewModel> UpdateRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleView = _mapper.Map<RoleViewModel>(role);
            _logger.LogInformation($"Роль для обновления: {roleView.Name}");

            return roleView;
        }

        public async Task UpdateRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.ID);
            role.Convert(model);

            await _roleManager.UpdateAsync(role);
            _logger.LogInformation($"Роль {role.Name} обновлена");
        }
    }
}
