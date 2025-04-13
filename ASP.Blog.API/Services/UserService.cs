using ASP.Blog.MVC.Controllers;
using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
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
        public async Task<List<UserViewRequest>> AllUsers()
        {
            _logger.LogInformation($"Вывод списка всех пользователей.");
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var users = repo.GetUsers();

            var usersView = new List<UserViewRequest>();

            foreach (var user in users)
            {
                var userView = _mapper.Map<UserViewRequest>(user);

                var userRoleNames = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.ToListAsync();
                var userRolesReq = new List<RoleRequest>();

                foreach (var role in allRoles)
                {
                    if (userRoleNames.Contains(role.Name))
                    {
                        userRolesReq.Add(new RoleRequest() { ID = role.Id, Name = role.Name, Description = role.Description });
                    }
                }
                userView.UserRoles = userRolesReq;

                usersView.Add(userView);
            }
            return usersView;
        }

        public void DeleteUser(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            repo.DeleteUser(user);
            _logger.LogInformation($"Пользователь с ID = {userId} удален.");
        }

        public async Task<SignInResult> Login(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return _ = SignInResult.Failed;
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            return result;
        }

        public void Register(RegisterRequest model)
        {
            throw new System.NotImplementedException();
        }

        public UserViewRequest UpdateUser(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            var userView = _mapper.Map<UserViewRequest>(user);
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

            //userView.CheckedRolesDic = checkedRolesDic;

            return userView;
        }

        public async Task UpdateUser(UserEditRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = new List<UserRole>();
            foreach (var role in roles)
            {
                if(userRoleNames.Contains(role.Name))
                { 
                    userRoles.Add(role); 
                }
            }

            var checkedModelRolesId = model.UserRoles.Select(x => x.ID).Intersect(roles.Select(x => x.Id)).ToList();

            var addRolesId = checkedModelRolesId.Except(userRoles.Select(x => x.Id));
            var delRolesId = userRoles.Select(x => x.Id).Except(checkedModelRolesId).ToList();

            // Очищаем
            //article.Tags.Clear();
            //Добавляем
            foreach (var role in roles)
            {
                if (addRolesId.Contains(role.Id))
                {
                    //article.Tags.Add(dbTag);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                if (delRolesId.Contains(role.Id))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            user.BirthDate = model.BirthDate; 
            user.Email = model.Email;
            user.First_Name = model.First_Name;
            user.Last_Name = model.Last_Name;
            user.Middle_Name = model.Middle_Name;
            user.UserName = model.Login;
            
            await _userManager.UpdateAsync(user);
            _logger.LogInformation($"Пользователь {user.UserName} обновлен.");
        }
    }
}
