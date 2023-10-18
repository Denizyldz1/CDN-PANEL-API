using AppouseProject.Core.Abstract.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppouseProject.Data.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
