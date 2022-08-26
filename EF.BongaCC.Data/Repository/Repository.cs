using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Data.Common;

/// <summary>
/// Summary description for Repository
/// </summary>

namespace EF.BongaCC.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BongaCCDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        //public readonly ILogger _logger;

        public Repository(BongaCCDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
            //_logger = logger;
        }

        public async Task<T> GetById(long? id)
        {
            return await entities.FindAsync(id);
            //return await entities.SingleOrDefault(s => s.ID == id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        //public async Task<IEnumerable<T>> GetByYear(int? iYear)
        //{
        //    return await entities.ToListAsync().Result.Where(o => o.AddedDate.Year == iYear);
        //}

        public async Task<long?> Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                await entities.AddAsync(entity);
                await context.SaveChangesAsync();
            }
            catch(DbException dbEx)
            {
                //foreach (var validationErrors in dbEx.GetBaseException())
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                //    }
                //}
                throw new Exception(errorMessage, dbEx);
            }

            return entity.ID;
        }

        public async Task<bool> Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            int i = await context.SaveChangesAsync();
            if (i == 1) return true;
            else return false;
        }

        public async Task<bool> Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);

            int i = await context.SaveChangesAsync();
            if (i == 1) return true;
            else return false;
        }

        public async Task<bool> DeleteRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.RemoveRange(entity);

            int i = await context.SaveChangesAsync();
            if (i == 1) return true;
            else return false;
        }
    }
}