using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Model.Data;

/// <summary>
/// Summary description for UnitOfWork
/// </summary>

namespace EF.BongaCC.Data.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly BongaCCDbContext context;
        private bool disposed;
        private Dictionary<string, object> repositories;

        public UnitOfWork(BongaCCDbContext context)
        {
            this.context = context;
        }

        //public UnitOfWork()
        //{
        //    context = new EFDbContext();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public Repository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }
    }
}