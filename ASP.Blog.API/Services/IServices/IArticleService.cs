using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Article;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface IArticleService
    {
        public int AddArticle(ArticleAddRequest article, User user);
        public List<ArticleViewRequest> AllUserArticles();
        public List<ArticleViewRequest> AllArticles(User user = null);
        public void DeleteArticle(int id);
        public void UpdateArticle(ArticleEditRequest article, User user);
    }
}
