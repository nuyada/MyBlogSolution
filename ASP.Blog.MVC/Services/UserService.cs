using ASP.Blog.MVC.Controllers;
using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.DTO;
using ASP.Blog.MVC.Extentions;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels;
using ASP.Blog.MVC.ViewModels.Role;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Services
{
    public class UserService : IUserService
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager,
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
        public async Task<List<UserViewModel>> AllUsers()
        {
            _logger.LogInformation($"Вывод списка всех пользователей.");
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var users = repo.GetUsers();

            var usersView = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userView = _mapper.Map<UserViewModel>(user);

                var userRoleNames = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.ToListAsync();
                var userRoles = new List<UserRole>();

                foreach (var role in allRoles)
                {
                    if (userRoleNames.Contains(role.Name))
                    {
                        userRoles.Add(role);
                    }
                }
                userView.UserRoles = userRoles;

                usersView.Add(userView);
            }
            return usersView;
        }

        public void DeleteUser(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);

            if (user == null)
            {
                _logger.LogWarning($"Не удалось удалить пользователя: пользователь с ID {userId} не найден.");
                return;
            }

            repo.DeleteUser(user);
            _logger.LogInformation($"Пользователь с ID = {userId} удален.");
        }

        public void Login(LoginViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public void Register(RegisterViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public UserViewModel UpdateUser(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            var userView = _mapper.Map<UserViewModel>(user);
            _logger.LogInformation($"Пользователь для обновления: {user.UserName}");

            var allRoles = _roleManager.Roles.ToList();
            var checkedRolesDic = new Dictionary<UserRole, bool>();
            foreach (var role in allRoles)
            {
                if (_userManager.IsInRoleAsync(user, role.Name).Result)
                {
                    checkedRolesDic.Add(role, true);
                }
                else
                {
                    checkedRolesDic.Add(role, false);
                }
            }

            userView.CheckedRolesDic = checkedRolesDic;

            return userView;
        }

        public async Task UpdateUser(UserViewModel model, List<string> SelectedRoles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                //определяем есть ли роль у пользователя
                var IsInRole = await _userManager.IsInRoleAsync(user, role.Name);

                //добавляем роль
                if (SelectedRoles.Contains(role.Id) && !IsInRole)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                //убираем роль
                if (!SelectedRoles.Contains(role.Id) && IsInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

            }

            user.Convert(model);
            await _userManager.UpdateAsync(user);
            _logger.LogInformation($"Пользователь {user.UserName} обновлен.");
        }
    }
}
