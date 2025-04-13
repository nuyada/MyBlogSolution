using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels.Comment;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.MVC.Controllers
{
   
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentService _commentService;

        public CommentController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<CommentController> logger,
                RoleManager<UserRole> roleManager,
                ICommentService commentService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _commentService = commentService;
        }
        /// <summary>
        /// Добавление комментария к статье
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// POST
        /// {
        ///  "comment": "string",
        ///  "articleId": 0
        /// }
        /// </remarks>
        /// <param name="model">Данные комментария для добавления</param>
        /// <returns></returns>

        [Route("AddComment")]
        [HttpGet]
        public IActionResult AddComment(int articleId)
        {
            //_logger.LogInformation($"Выполняется переход на страницу добавления комментария для статьи с ID = {articleId}");
            //return View(new CommentViewModel() { ArticleId = articleId} );
            return View(_commentService.AddComment(articleId));
        }

        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            // Получаем пользователя до валидации
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.UserId = user.Id;
            ModelState.Remove(nameof(model.UserId));
            if (ModelState.IsValid)
            {
                _commentService.AddComment(model, user);
                return RedirectToAction("ViewArticle", "Article", new { Id = model.ArticleId });
            }

            _logger.LogError("Модель CommentViewModel не прошла проверку при добавлении комментария.");
            ModelState.AddModelError("", "Ошибка в модели!");
            return View(model);
        }
        /// <summary>
        /// Вывод всех комментарие к статье
        /// </summary>
        /// <param name="articleId"> Id статьи </param>
        /// <returns> Список комментариев </returns>
        [Route("AllArticleComments")]
        [HttpGet]
        public IActionResult AllArticleComments(int articleId)
        {
            //_logger.LogInformation($"Выполняется переход на страницу просмотра всех статей комментариев статьи с ID = {articleId}.");
            //var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            //var comments = repo.GetComments();
            //var commentsView = new List<CommentViewModel>();
            //foreach (var comment in comments) 
            //{
            //    if (comment.ArticleId == articleId)
            //    {
            //        commentsView.Add(_mapper.Map<CommentViewModel>(comment));
            //    }
            //}

            //return View(commentsView);
            return View(_commentService.AllArticleComments(articleId));
        }
        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id"> Id комментария</param>
        /// <returns></returns>
        [Route("Comment/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            //var repo =_unitOfWork.GetRepository<Comment>() as CommentRepository;
            //var comment = repo.GetCommentById(id);
            //var articleId = comment.ArticleId;
            //_logger.LogInformation($"Удаление комментария с ID = {id}");

            //repo.Delete(comment);

            //return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
            var comment = _commentService.GetCommentEntityById(id);
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (comment.UserId != currentUser.Id && !isAdmin)
            {
                return Forbid();
            }
            var articleId = _commentService.DeleteComment(id);
            if (articleId is not null)
            {
                return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
            }
            else
            {
                return RedirectToAction("AllArticles", "Article");
            }

        }
        [Route("Comment/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var comment = _commentService.GetCommentEntityById(id);
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (comment.UserId != currentUser.Id && !isAdmin)
            {
                return Forbid();
            }

            return View("EditComment", _commentService.UpdateComment(id));
        }
        [Route("Comment/Update")]
        [HttpPost]
        public IActionResult Update(CommentViewModel model)
        {
            int articleId;

            if (ModelState.IsValid)
            {
                //var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
                //var comment = repo.GetCommentById(model.Id);
                //comment.Convert(model);
                //articleId = comment.ArticleId;

                //repo.Update(comment);
                articleId = _commentService.UpdateComment(model);
                _logger.LogInformation($"Выполняется переход на страницу просмотра статьи c ID = {articleId.ToString()}");
                return RedirectToAction("ViewArticle", "Article", new { Id = articleId });
            }
            else
            {
                _logger.LogError("Модель CommentViewModel при обновлении комментария невалидна!");
                ModelState.AddModelError("", "Ошибка в модели!");
                _logger.LogWarning($"Выполняется переход на страницу просмотра всех статей.");
                return RedirectToAction("AllUserArticles", "Article");
            }
        }
    }
}
