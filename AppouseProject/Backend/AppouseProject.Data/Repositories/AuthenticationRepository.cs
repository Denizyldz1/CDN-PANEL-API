using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppouseProject.Data.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<UserRefreshToken> _dbSet;

        public AuthenticationRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<UserRefreshToken>();
        }
        public async Task AddAsync(UserRefreshToken token)
        {
            await _dbSet.AddAsync(token);
        }

        public void Remove(UserRefreshToken token)
        {
            _dbSet.Remove(token);
        }

        public IQueryable<UserRefreshToken> Where(Expression<Func<UserRefreshToken, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
