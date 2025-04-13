using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Tag
{
    public class TagAddRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Tag_Name { get; set; }
    }
}
