using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.QuatoDtos;
using AppouseProject.Core.Entities;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IQuotaService
    {
        Task<CustomResponseDto<QuotaDto>> QuotaByUserId(int userId);
        Task<CustomResponseDto<NoContentDto>> QuotaIncrease(int userId, int amount);
        Task<CustomResponseDto<NoContentDto>> QuotaReduction(int userId, int amount);
        Task<CustomResponseDto<IEnumerable<QuotaDto>>> GetAllAsync();
    }
}
