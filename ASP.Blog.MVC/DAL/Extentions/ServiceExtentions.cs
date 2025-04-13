using ASP.Blog.MVC.DAL.Repositories;
using ASP.Blog.MVC.DAL.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Blog.MVC.DAL.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services) 
        { 
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            return services;
        }
        public static IServiceCollection AddCustomRepository<TEntity, IRepository>(this IServiceCollection services) 
            where TEntity : class
            where IRepository : class, IRepository<TEntity>
        {
            services.AddScoped<IRepository<TEntity>, IRepository>();
            return services;
        }
    }
}
