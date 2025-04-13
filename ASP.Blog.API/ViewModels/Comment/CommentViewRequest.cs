using System;
using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Comment
{
    public class CommentViewRequest
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CommentDate { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string UserId { get; set; }
    }
}
