using System.Collections.Generic;

namespace ASP.Blog.MVC.Data.Entities
{
    public class Tag
    {
        public int ID { get; set; }
        public string Tag_Name { get; set; }
        public List<Article> Articles { get; set; }
    }
}
