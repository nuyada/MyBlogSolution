using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Article;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface IArticleService
    {
        public ArticleViewModel AddArticle(User user);
        public void AddArticle(ArticleViewModel article, List<int> SelectedTags, User user);
        public List<ArticleViewModel> AllUserArticles();
        public List<ArticleViewModel> AllArticles(User user = null);
        public ArticleViewModel ViewArticle(int id);
        public void DeleteArticle(int id);
        public ArticleViewModel UpdateArticle(int id, User user);
        public void UpdateArticle(ArticleViewModel article, List<int> SelectedTags, User user);
    }
}
