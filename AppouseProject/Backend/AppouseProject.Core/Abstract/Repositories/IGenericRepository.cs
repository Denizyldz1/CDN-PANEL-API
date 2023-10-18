using System.Linq.Expressions;

namespace AppouseProject.Core.Abstract.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);


    }
}
