using ASP.Blog.MVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Blog.MVC.DAL.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogContext db) : base(db) { }

        public List<Tag> GetTags()
        {
            return Set.ToList();
        }
        public Tag GetTagById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddTag(Tag tag)
        {
            Set.Add(tag);
        }
        public void UpdateTag(Tag tag)
        {
            Update(tag);
        }
        public void DeleteTag(Tag tag)
        {
            Delete(tag);
        }
    }
}
