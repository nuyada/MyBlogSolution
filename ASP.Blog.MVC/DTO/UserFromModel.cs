using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.ViewModels;

namespace ASP.Blog.MVC.DTO
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserViewModel usereditvm)
        {
            user.Last_Name = usereditvm.Last_Name;
            user.Middle_Name = usereditvm.Middle_Name;
            user.First_Name = usereditvm.First_Name;
            user.Email = usereditvm.Email;
            //user.BirthDate = usereditvm.BirthDate;
            user.BirthDate = new System.DateTime((int)usereditvm.Year, (int)usereditvm.Month, (int)usereditvm.Day);

            return user;
        }
    }
}
