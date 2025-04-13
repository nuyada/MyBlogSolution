using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Data.Entities
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ArticleDate { get; set; }
        [Comment("This field contains text of an article")]
        public string Content { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
