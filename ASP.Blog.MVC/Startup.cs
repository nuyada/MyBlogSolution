using ASP.Blog.MVC.DAL.Entities;
using ASP.Blog.MVC.DAL.Extentions;
using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.Data.Entities;
using ASP.Blog.MVC.Services;
using ASP.Blog.MVC.Services.IServices;
using ASP.Blog.MVC.Validators;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore; // Для работы с SQLite
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ASP.Blog.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = Configuration.GetConnectionString("DefaultConnection");
            string connection = Configuration.GetConnectionString("HP_Connection");
            var mapperConfig = new MapperConfiguration(v =>
            {
                v.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            //services.AddSingleton<ILogger, Logger>();

            services
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ArticleViewModelValidator>())
                .AddDbContext<BlogContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))) // Подключение к SQLite
                .AddUnitOfWork()
                .AddCustomRepository<Article, ArticleRepository>()
                .AddTransient<IArticleService, ArticleService>()
                .AddCustomRepository<Comment, CommentRepository>()
                .AddTransient<ICommentService, CommentService>()
                .AddCustomRepository<Tag, TagRepository>()
                .AddTransient<ITagService, TagService>()
                .AddCustomRepository<Article_Tags, Article_TagsRepository>()
                .AddCustomRepository<User, UserRepository>()
                .AddTransient<IUserService, UserService>()
                .AddCustomRepository<UserRole, UserRoleRepository>()
                .AddTransient<IRoleService, RoleService>()
                .AddIdentity<User, UserRole>(opts =>
                {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<BlogContext>()
            ;
            services.AddAuthentication(options => options.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/Home/Error";
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            var cachePeriod = "0";
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age = {cachePeriod}");
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
