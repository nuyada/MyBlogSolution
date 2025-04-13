using ASP.Blog.MVC.DAL.Repositories;
using System;

namespace ASP.Blog.MVC.DAL.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(bool ensureAutoHistory = false);
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
