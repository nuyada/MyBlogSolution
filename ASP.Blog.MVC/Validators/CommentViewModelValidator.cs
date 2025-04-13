using ASP.Blog.MVC.ViewModels.Comment;
using FluentValidation;

namespace ASP.Blog.MVC.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator() 
        { 
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Комментарий не должен быть пуст!");
        }
    }
}
