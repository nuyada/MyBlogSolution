using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ASP.Blog.MVC.Data.Entities;

namespace ASP.Blog.MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<User> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Глобальный обработчик ошибок
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        [HttpGet]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue) 
            {
                _logger.LogError($"Произошла ошибка с кодом: {statusCode}");

                if (statusCode == 404)
                {
                    return StatusCode(404);
                }
                else if (statusCode == 403)
                {
                    return StatusCode(403);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            else
            {
                _logger.LogInformation($"Произошла ошибка, код ошибки неизвестен...");
                return StatusCode(500);
            }
        }
    }
}
