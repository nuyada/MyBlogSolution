using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Comment;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface ICommentService
    {
        public CommentViewModel AddComment(int articleId);
        public void AddComment(CommentViewModel model, User user);
        public List<CommentViewModel> AllArticleComments(int articleId);
        public int? DeleteComment(int id);
        public CommentViewModel UpdateComment(int id);
        public int UpdateComment(CommentViewModel model);
        
        Comment GetCommentEntityById(int id);


    }
}
