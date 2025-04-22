using System;
using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
