using System;

namespace ASP.Blog.MVC.Data.Entities
{
    public class Article_Tags
    {
        public int ID { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
