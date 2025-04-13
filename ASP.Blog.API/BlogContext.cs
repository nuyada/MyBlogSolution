using ASP.Blog.MVC.DAL.Configs;
using ASP.Blog.MVC.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.Blog.MVC
{
    public class BlogContext : IdentityDbContext<User>
    {
        override public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Article_Tags> Article_Tags { get; set; }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
            //Database.EnsureCreated();
            //Database.EnsureDeleted();
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ArticleConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            //builder.ApplyConfiguration<Article_Tags>(new Article_TagsConfiguration());
        }
    }
}
