using Entity = ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using ASP.Blog.MVC.ViewModels.Tag;

namespace ASP.Blog.MVC.ViewModels.Article
{
    public class ArticleAddRequest
    {
        [Required(ErrorMessage = "Поле Заголовок статьи обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок статьи", Prompt = "Заголовок статьи")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле дата статьи обязательно для заполнения")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата статьи", Prompt = "Дата статьи")]
        public DateTime ArticleDate { get; set; }
        [Required(ErrorMessage = "Поле Содержимое статьи обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Содержимое статьи", Prompt = "Содержимое статьи")]
        public string Content { get; set; }
        [Display(Name = "Теги статьи", Prompt = "Теги статьи")]
        public List<TagRequest> Tags { get; set; }
    }
}
