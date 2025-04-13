using ASP.Blog.MVC.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ASP.Blog.MVC.Data.Entities
{
    public class User : IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle_Name { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Article> Articles { get; set; }
        public UserRole userRole { get; set; }
    }
}
