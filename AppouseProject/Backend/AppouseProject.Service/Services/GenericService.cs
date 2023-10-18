using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Service.Services
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public virtual IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }
    }
}
