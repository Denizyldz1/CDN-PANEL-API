using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Entities;

namespace AppouseProject.Data.Repositories
{
    public class AuthenticationRepository : GenericRepository<UserRefreshToken> , IAuthenticationRepository
    {
        public AuthenticationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(UserRefreshToken token)
        {
            await _dbSet.AddAsync(token);
        }

        public void Remove(UserRefreshToken token)
        {
            _dbSet.Remove(token);
        }
    }
}
