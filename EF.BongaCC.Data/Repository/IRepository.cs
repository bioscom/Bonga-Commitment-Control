using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EF.BongaCC.Data.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(long? id);
        Task<IEnumerable<T>> GetAll();
        //Task<IEnumerable<T>> GetByYear(int? iYear);
        Task<long?> Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> DeleteRange(IEnumerable<T> entity);
    }
}
