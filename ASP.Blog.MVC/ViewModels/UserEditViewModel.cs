using ASP.Blog.MVC.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID", Prompt = "ID пользователя")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string First_Name { get; set; }
        [Required]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        [Required(ErrorMessage = "Поле Email обязательно к заполнению")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения", Prompt = "Укажите вашу дату рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Description = "Введите логин")]
        public string Login { get; set; }

        public List<UserRole> UserRoles { get; set; }

    }
}
