using AppouseProject.Core.Entities;
using System.Linq.Expressions;

namespace AppouseProject.Core.Abstract.Repositories
{
    public interface IAuthenticationRepository
    {
        IQueryable<UserRefreshToken> Where(Expression<Func<UserRefreshToken, bool>> expression);
        Task AddAsync(UserRefreshToken token);
        void Remove(UserRefreshToken token);



    }
}
