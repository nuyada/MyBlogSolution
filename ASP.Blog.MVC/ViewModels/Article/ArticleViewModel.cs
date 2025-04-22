using Entity = ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ASP.Blog.MVC.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Title { get; set; }
        public DateTime ArticleDate { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Content { get; set; }
        [BindNever]
        public Entity.User? User { get; set; }
        [BindNever]
        public List<Entity.Tag>? Tags { get; set; } = new();
        public List<int> SelectedTags { get; set; } = new();
        [BindNever]
        public List<Entity.Comment>? Comments { get; set; }
        [BindNever]
        public Dictionary<Entity.Tag, bool>? CheckedTagsDic { get; set; }
        public ArticleViewModel(Entity.User user) => User = user;
        public ArticleViewModel() { }
    }
}
