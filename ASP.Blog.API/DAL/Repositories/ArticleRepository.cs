using ASP.Blog.MVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Blog.MVC.DAL.Repositories
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogContext db) : base(db) { }
        public List<Article> GetArticles()
        {
            return Set.Include(t => t.Tags).ToList();
        }
        public Article GetArticleById(int id)
        {
            return Set.Include(c => c.Tags).Include(c => c.Comments).ThenInclude(cu => cu.User).Include(u => u.User).Where(x => x.ID == id).FirstOrDefault();
        }
        public List<Article> GetArticlesByUserId(string userId)
        {
            return Set.Include(c => c.Tags).Where(x => x.UserId == userId).ToList();
        }
        public void AddArticle(Article article)
        {
            Create(article);
        }
        public void UpdateArticle(Article article)
        {
            Update(article);
        }
        public void DeleteArticle(Article article)
        {
            Delete(article);
        }
    }
}
