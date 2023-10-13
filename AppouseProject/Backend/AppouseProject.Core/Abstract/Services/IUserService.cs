using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Entities;
using System.Linq.Expressions;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IUserService
    {
        // Sonradan eklenen düzeltmeler
        Task<CustomResponseDto<AppUserWithRoleModel>> CreateUserAsync(SignInModel model);
        Task<CustomResponseDto<AppUserWithRoleModel>> GetUserByNameAsync(string userName);

        Task<CustomResponseDto<AppUserWithRoleModel>> GetByIdAsync(int id);
        Task<CustomResponseDto<IEnumerable<AppUserWithRoleModel>>> GetAllAsync();
        Task<bool> AnyAsync(Expression<Func<AppUser, bool>> expression);
        Task<CustomResponseDto<NoContentDto>> ChangeEmailAsync(string userName, string newEmail);
        Task<CustomResponseDto<NoContentDto>> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
        Task<CustomResponseDto<NoContentDto>> ChangeRoleAsync(string userId, string newRole);
    }
}
