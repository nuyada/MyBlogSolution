using ASP.Blog.MVC.Controllers;
using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.DAL.UoW;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.ViewModels.Comment;
using ASP.Blog.BLL.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Services
{
    public class CommentService : ICommentService
    {
        private IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleService _articleService;

        public CommentService(ILogger<ArticleController> logger,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper,
                RoleManager<UserRole> roleManager,
                IArticleService articleService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _articleService = articleService;
        }
        public CommentViewModel AddComment(int articleId)
        {
            _logger.LogInformation($"Выполняется переход на страницу добавления комментария для статьи с ID = {articleId}");
            return new CommentViewModel() { ArticleId = articleId };
        }

        public void AddComment(CommentAddRequest model, User user)
        {
            var comment = _mapper.Map<Comment>(model);

            comment.CommentDate = DateTime.Now;
            comment.User = user;
            comment.UserId = comment.User.Id;
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            repo.Create(comment);
            _logger.LogInformation($"Комментарий создал пользователь {comment.User.UserName} : {comment.User.First_Name} {comment.User.Last_Name}");
        }

        public List<CommentViewRequest> AllArticleComments(int articleId)
        {
            _logger.LogInformation($"Выполняется переход на страницу просмотра всех статей комментариев статьи с ID = {articleId}.");
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetComments();
            var commentsView = new List<CommentViewRequest>();
            foreach (var comment in comments)
            {
                if (comment.ArticleId == articleId)
                {
                    commentsView.Add(_mapper.Map<CommentViewRequest>(comment));
                }
            }
            return commentsView;
        }

        public int? DeleteComment(int id)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            _logger.LogInformation($"Удаление комментария с ID = {id}");

            repo.Delete(comment);

            return comment.ArticleId;
        }

        public CommentViewModel UpdateComment(int id)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(id);
            var commentView = _mapper.Map<CommentViewModel>(comment);
            _logger.LogInformation($"Выполняется переход на страницу обновления комментария с ID = {id}. ID статьи = {comment.ArticleId}.");

            return commentView;
        }

        public int UpdateComment(CommentEditRequest model, User user)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = repo.GetCommentById(model.Id);
            comment.Comment_Text = model.Comment;
            comment.User = user;

            repo.Update(comment);

            return comment.ArticleId;
        }
    }
}
