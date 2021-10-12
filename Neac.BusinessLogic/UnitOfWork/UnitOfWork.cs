using Neac.BusinessLogic.BaseRepository;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private NeacDbContext _dbFactory;

        public NeacDbContext DbContext { get { return _dbFactory; } }
        public IRepository<T> GetRepository<T>() where T :class => new Repository<T>(_dbFactory);
        public UnitOfWork(NeacDbContext dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<int> SaveAsync()
        {
            return await _dbFactory.SaveChangesAsync();
        }
    }
}
