using ASP.Blog.MVC.ViewModels.Tag;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Services.IServices
{
    public interface ITagService
    {
        public void AddTag(TagAddRequest model);
        public List<TagRequest> AllTags();
        public void DeleteTag(int id);
        public void UpdateTag(TagRequest model);

    }
}
