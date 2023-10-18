using AppouseProject.Core.Entities;
using System.Linq.Expressions;

namespace AppouseProject.Core.Abstract.Repositories
{
    public interface IAuthenticationRepository : IGenericRepository<UserRefreshToken>
    {
        Task AddAsync(UserRefreshToken token);
        void Remove(UserRefreshToken token);



    }
}
