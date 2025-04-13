using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Article;

namespace ASP.Blog.MVC.Extentions
{
    public static class ArticleFromModel
    {
        public static Article Convert(this Article article, ArticleViewModel articleViewModel)
        {
            article.ID = articleViewModel.Id;
            article.Title = articleViewModel.Title;
            article.Content = articleViewModel.Content;
            article.ArticleDate = articleViewModel.ArticleDate;
            article.UserId = articleViewModel.User.Id;                  
            article.User = articleViewModel.User;
            article.Tags = articleViewModel.Tags;

            return article;
        }
    }
}
