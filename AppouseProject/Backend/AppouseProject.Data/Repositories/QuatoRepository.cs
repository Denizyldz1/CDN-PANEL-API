using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Entities;
using AppouseProject.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Data.Repositories
{
    public class QuotaRepository : IQuotaRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Quota> _quotaSet;

        public QuotaRepository(AppDbContext context)
        {
            _context = context;
            _quotaSet =_context.Set<Quota>();
        }

        public async Task AddAsync(Quota quota)
        {
            await _quotaSet.AddAsync(quota);
        }

        public IQueryable<Quota> GetAll()
        {
           return _quotaSet.AsNoTracking().AsQueryable();
        }

        public async Task<Quota> QuotaByUserId(int userId)
        {
           return await _quotaSet.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public  void QuotaIncrease(int userId, int amount)
        {
            var value =  _quotaSet.Find(userId);
            value.UsedSpaceByte = value.UsedSpaceByte + amount;
        }

        public  void QuotaReduction(int userId, int amount)
        {
            var value = _quotaSet.Find(userId);
            value.UsedSpaceByte = value.UsedSpaceByte - amount;
        }
        public void QuotaUpdate(int userId, int amount, string userType)
        {
            var value = _quotaSet.Find(userId);
            if (userType == UserType.Standart.ToString())
            {
                value.StorageSpaceByte = value.StorageSpaceByte - amount;
            }
            else { value.StorageSpaceByte = value.StorageSpaceByte + amount; }
           
        }
    }
}
