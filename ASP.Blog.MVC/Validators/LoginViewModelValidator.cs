using ASP.Blog.MVC.ViewModels;
using FluentValidation;

namespace ASP.Blog.MVC.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginRequest>
    {
        public LoginViewModelValidator() 
        { 
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email не должен быть пуст!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль не должен быть пуст!");
        }
    }
}
