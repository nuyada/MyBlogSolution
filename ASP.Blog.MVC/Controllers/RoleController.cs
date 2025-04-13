using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels.Role;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Controllers
{
    
    public class RoleController : Controller
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;

        public RoleController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager,
                IRoleService roleService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _roleService = roleService;
        }
        [Route("Role/AddRole")]
        [HttpGet]
        public IActionResult AddRole()
        {
            //_logger.LogInformation("Выполняется переход на страницу добавления роли.");
            //return View(new RoleViewModel());
            return View(_roleService.AddRole());
        }
        [Route("Role/AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ////Инициализируем так, чтобы заполнить ID
                //var role = new UserRole();

                //var roleData = _mapper.Map<UserRole>(model);
                //role.Name = roleData.Name;
                //role.Description = roleData.Description;

                //await _roleManager.CreateAsync(role);
                //_logger.LogInformation($"Создана роль {role.Name}");
                await _roleService.AddRole(model);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            _logger.LogInformation($"Выполняется переход на страницу просмотра всех ролей");
            return RedirectToAction("AllRoles", "Role");
        }

        [Route("AllRoles")]
        [HttpGet]
        public IActionResult AllRoles()
        {
            
            return View(_roleService.AllRoles());
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(string roleId)
        {
            

            var roleView = await _roleService.UpdateRole(roleId);
            return View("EditRole", roleView);
            //return View("EditRole", _roleService.UpdateRole(roleId));
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                await _roleService.UpdateRole(model);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                ModelState.AddModelError("", "Некорректные данные");
            }
            _logger.LogInformation($"Перенаправление на страницу просмотра всех ролей");

            return RedirectToAction("AllRoles");
        }
        [Authorize(Roles = "Admin")]
        [Route("Role/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            
            
            await _roleService.DeleteRole(roleId);

            return RedirectToAction("AllRoles");
        }
    }
}
