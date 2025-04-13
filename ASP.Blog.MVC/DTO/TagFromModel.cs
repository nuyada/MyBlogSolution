using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels.Tag;

namespace ASP.Blog.MVC.Extentions
{
    public static class TagFromModel
    {
        public static Tag Convert(this Tag tag, TagViewModel tagViewModel)
        {
            tag.ID = tagViewModel.Id;
            tag.Tag_Name = tagViewModel.Tag_Name;

            return tag;
        }
    }
}
