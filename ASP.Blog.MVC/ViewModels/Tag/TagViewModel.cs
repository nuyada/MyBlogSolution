using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Tag
{
    public class TagViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Text)]
        public string Tag_Name { get; set; }
    }
}
