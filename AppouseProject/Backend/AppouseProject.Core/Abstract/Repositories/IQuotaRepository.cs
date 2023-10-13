using AppouseProject.Core.Entities;

namespace AppouseProject.Core.Abstract.Repositories
{
    public interface IQuotaRepository
    {
        Task<Quota> QuotaByUserId(int userId);
        void QuotaIncrease(int userId, int amount);
        void QuotaReduction(int userId, int amount);
        void QuotaUpdate(int userId, int amount, string userType);
        IQueryable<Quota> GetAll();
        Task AddAsync(Quota quota);
    }
}
