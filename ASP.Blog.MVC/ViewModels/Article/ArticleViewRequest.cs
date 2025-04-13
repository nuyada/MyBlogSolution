using Entity = ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using ASP.Blog.MVC.ViewModels.Tag;

namespace ASP.Blog.MVC.ViewModels.Article
{
    public class ArticleViewRequest
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public DateTime ArticleDate { get; set; }
        public string Content { get; set; }
        public List<TagRequest> Tags { get; set; }
    }
}
