using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Comment;

namespace ASP.Blog.BLL.Extentions
{
    public static class CommentFromModel
    {
        public static Comment Convert(this Comment comment, CommentViewModel commentViewModel)
        {
            comment.ID = commentViewModel.Id;
            comment.ArticleId = commentViewModel.ArticleId;
            comment.UserId = commentViewModel.UserId;
            comment.CommentDate = commentViewModel.CommentDate;
            comment.Comment_Text = commentViewModel.Comment;

            return comment;
        }
    }
}
