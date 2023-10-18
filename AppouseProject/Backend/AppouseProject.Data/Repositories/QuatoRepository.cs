using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Entities;
using AppouseProject.Core.EntityConst;
using AppouseProject.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Data.Repositories
{
    public class QuotaRepository : GenericRepository<Quota>, IQuotaRepository
    {
        public QuotaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Quota quota)
        {
            await _dbSet.AddAsync(quota);
        }

        public IQueryable<Quota> GetAll()
        {
           return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<Quota> QuotaByUserId(int userId)
        {
           return await _dbSet.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public  void QuotaIncrease(int userId, int amount)
        {
            var value =  _dbSet.Find(userId);
            value.UsedSpaceByte = value.UsedSpaceByte + amount;
        }

        public  void QuotaReduction(int userId, int amount)
        {
            var value = _dbSet.Find(userId);
            value.UsedSpaceByte = value.UsedSpaceByte - amount;
        }
        public void QuotaUpdate(int userId, int amount, string userType)
        {
            var value = _dbSet.Find(userId);
            if (userType == UserType.Standart.ToString())
            {
                value.StorageSpaceByte = (int)UserQuota.Standart;
            }
            else { value.StorageSpaceByte = (int)UserQuota.Premium; }
           
        }
    }
}
