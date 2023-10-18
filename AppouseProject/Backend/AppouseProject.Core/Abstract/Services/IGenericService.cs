using System.Linq.Expressions;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IGenericService<T>
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
       IQueryable<T> Where(Expression<Func<T, bool>> expression);


    }
}
