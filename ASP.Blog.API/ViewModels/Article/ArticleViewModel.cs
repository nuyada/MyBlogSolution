using Entity = ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System;

namespace ASP.Blog.MVC.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ArticleDate { get; set; }
        public string Content { get; set; }
        public Entity.User User { get; set; }
        public List<Entity.Tag> Tags { get; set; }
        public List<int> SelectedTags { get; set; }
        public List<Entity.Comment> Comments { get; set; }
        public Dictionary<Entity.Tag, bool> CheckedTagsDic { get; set; }
        public ArticleViewModel(Entity.User user) => User = user;
        public ArticleViewModel() { }
    }
}
