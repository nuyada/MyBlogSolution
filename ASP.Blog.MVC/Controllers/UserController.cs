using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Controllers
{
    
    public class UserController : Controller
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager,
                IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userService = userService;
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Регистрация нового пользователя.");
            return View(new RegisterViewModel());
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    // ✅ назначаем роль по имени, если она уже есть в БД
                    if (await _roleManager.RoleExistsAsync("User"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation($"Пользователь {user.Last_Name} {user.First_Name} зарегистрирован.");

                    return RedirectToAction("Index", "Home");
                }

                // ошибки регистрации
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                User signedUser = _userManager.Users.Include(x => x.userRole).FirstOrDefault(u => u.Email == model.Email);
                var userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                if (signedUser is null)
                {
                    _logger.LogError($"Логин {user.Email} не найден");
                    ModelState.AddModelError("", "Неверный логин!");
                }
                /// Если ролей почему-то нет, то устанавливаем:
                /// для пользователя Admin - роль Admin
                /// для остальных - User
                if (userRole is null)
                {
                    _logger.LogError($"У пользователя {signedUser.UserName} нет роли!");
                    if (signedUser.UserName == "Admin")
                    {
                        await _userManager.AddToRoleAsync(signedUser, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(signedUser, "User");
                    }
                    userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                    _logger.LogWarning($"Пользователю {signedUser.userRole} присвоили роль {userRole}");
                }

                if (signedUser != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                    };

                    await _signInManager.SignInWithClaimsAsync(signedUser, isPersistent: false, claims);
                }
                else
                {
                    _logger.LogError($"Логин {user.Email} не найден");
                    ModelState.AddModelError("", $"Логин {user.Email} не найден");
                }
            }
            _logger.LogInformation($"Перенаправление на главную страницу.");
            return RedirectToAction("Index", "Home");
        }
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"Выполнен Logout.");

            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        [Route("AllUsers")]
        [HttpGet]
        public async Task<IActionResult> AllUsersAsync()
        {
           
            return View(await _userService.AllUsers());
        }

        [Authorize]
        [Route("User/Update")]
        [HttpGet]
        public IActionResult Update(string userId)
        {
            
            return View("EditUser", _userService.UpdateUser(userId));
        }

        [Authorize]
        [Route("User/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserViewModel model, List<string> SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                
                await _userService.UpdateUser(model, SelectedRoles);
            }
            else
            {
                _logger.LogError("Модель UserViewModel не прошла проверку!");
                ModelState.AddModelError("", "Некорректные данные");
            }
            _logger.LogInformation($"Перенаправление на страницу просмотра всех пользователей");

            return RedirectToAction("AllUserArticles", "Article");
        }
        [Authorize]
        [Route("User/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"Пользователь с ID = {userId} не найден.");
                return RedirectToAction("Index", "Home");
            }

            // Если пользователь удаляет сам себя — выходим из аккаунта
            if (User.Identity.Name == user.UserName)
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"Пользователь {user.UserName} удаляет свой аккаунт — выполняется SignOut.");
            }

            _userService.DeleteUser(userId);

            return RedirectToAction("Index", "Home");
        }
    }
}
 