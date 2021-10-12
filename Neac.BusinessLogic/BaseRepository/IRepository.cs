using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.BaseRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<List<T>> AddRangeAsync(List<T> entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity);
        Task<IQueryable<T>> AddRangeAsync(IQueryable<T> entity);
        Task<T> Delete(T entity);
        Task<T> Update(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetByExpression(Expression<Func<T, bool>> expression);
        Task<bool> DeleteByExpression(Expression<Func<T, bool>> expression);
    }
}
