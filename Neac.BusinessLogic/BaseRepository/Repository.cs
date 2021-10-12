using Microsoft.EntityFrameworkCore;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.BaseRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly NeacDbContext _neacDbContext;
        private DbSet<T> _dbSet;

        public Repository(NeacDbContext NeacDbContext)
        {
            _neacDbContext = NeacDbContext;
            _dbSet = _neacDbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<IQueryable<T>> AddRangeAsync(IQueryable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }
        public async Task<bool> DeleteByExpression(Expression<Func<T, bool>> expression)
        {
            try
            {
                var source = GetByExpression(expression);
                _dbSet.RemoveRange(source);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetByExpression(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
