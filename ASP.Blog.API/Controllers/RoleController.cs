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
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
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
        /// <summary>
        /// Добавление роли
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "name": "string",
        ///  "description": "string"
        ///}
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.AddRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод всех ролей
        /// </summary>
        /// <returns></returns>
        [Route("AllRoles")]
        [HttpGet]
        public List<RoleRequest> AllRoles()
        {
            return _roleService.AllRoles();
        }
        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "id": "string",
        ///  "name": "string",
        ///  "description": "string"
        ///}
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.UpdateRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Удаление роли
        /// Доступно только пользователям с ролью Admin
        /// </summary>
        /// <param name="roleId"> Id роли </param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            try
            {
                await _roleService.DeleteRole(roleId);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
    }
}
