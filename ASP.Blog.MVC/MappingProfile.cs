using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels;
using ASP.Blog.MVC.ViewModels.Article;
using ASP.Blog.MVC.ViewModels.Comment;
using ASP.Blog.MVC.ViewModels.Role;
using ASP.Blog.MVC.ViewModels.Tag;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.MVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(c => new DateTime((int)c.Year, (int)c.Month, (int)c.Day)))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));

            CreateMap<LoginViewModel, User>();

            CreateMap<UserViewModel, User>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.First_Name, opt => opt.MapFrom(c => c.First_Name))
                .ForMember(x => x.Last_Name, opt => opt.MapFrom(c => c.Last_Name))
                .ForMember(x => x.Middle_Name, opt => opt.MapFrom(c => c.Middle_Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login))
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(c => new DateTime((int)c.Year, (int)c.Month, (int)c.Day)));

            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.First_Name, opt => opt.MapFrom(c => c.First_Name))
                .ForMember(x => x.Last_Name, opt => opt.MapFrom(c => c.Last_Name))
                .ForMember(x => x.Middle_Name, opt => opt.MapFrom(c => c.Middle_Name))
                .ForMember(x => x.Login, opt => opt.MapFrom(c => c.UserName))
                .ForMember(x => x.Year, opt => opt.MapFrom(c => c.BirthDate.Year))
                .ForMember(x => x.Month, opt => opt.MapFrom(c => c.BirthDate.Month))
                .ForMember(x => x.Day, opt => opt.MapFrom(c => c.BirthDate.Day))
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email));

            CreateMap<User, UserEditViewModel>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));

            CreateMap<UserRole, RoleViewModel>()
                .ForMember(x => x.ID, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description));

            CreateMap<RoleViewModel, UserRole>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description));

            CreateMap<ArticleViewModel, Article>()
                .ForMember(x => x.Content, opt => opt.MapFrom(c => c.Content))
                .ForMember(x => x.User, opt => opt.MapFrom(c => c.User))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.User.Id))
                .ForMember(x => x.ArticleDate, opt => opt.MapFrom(c => c.ArticleDate))
                .ForMember(x => x.Title, opt => opt.MapFrom(c => c.Title))
                .ForMember(x => x.Tags, opt => opt.MapFrom(c => c.Tags));

            CreateMap<Article, ArticleViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Content, opt => opt.MapFrom(c => c.Content))
                .ForMember(x => x.User, opt => opt.MapFrom(c => c.User))
                .ForMember(x => x.ArticleDate, opt => opt.MapFrom(c => c.ArticleDate))
                .ForMember(x => x.Title, opt => opt.MapFrom(c => c.Title))
                .ForMember(x => x.Tags, opt => opt.MapFrom(c => c.Tags));

            CreateMap<Tag, TagViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Tag_Name, opt => opt.MapFrom(c => c.Tag_Name));

            CreateMap<TagViewModel, Tag>()
                .ForMember(x => x.ID, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Tag_Name, opt => opt.MapFrom(c => c.Tag_Name));

            CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.ID))
                .ForMember(x => x.Comment, opt => opt.MapFrom(c => c.Comment_Text))
                .ForMember(x => x.CommentDate, opt => opt.MapFrom(c => c.CommentDate))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.ArticleId, opt => opt.MapFrom(c => c.ArticleId));

            CreateMap<CommentViewModel, Comment>()
                .ForMember(x => x.ID, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Comment_Text, opt => opt.MapFrom(c => c.Comment))
                .ForMember(x => x.CommentDate, opt => opt.MapFrom(c => c.CommentDate))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.ArticleId, opt => opt.MapFrom(c => c.ArticleId));
        }
    }
}
