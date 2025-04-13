using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Role
{
    public class RoleRequest
    {
        public string ID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)] 
        public string Description { get; set; }
    }
}
