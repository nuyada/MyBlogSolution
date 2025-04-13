using System;

namespace ASP.Blog.MVC.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
