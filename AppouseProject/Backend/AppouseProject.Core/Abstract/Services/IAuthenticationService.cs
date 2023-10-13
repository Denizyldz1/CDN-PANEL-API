using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Dtos.TokenModels;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IAuthenticationService
    {
        // Kullanıcı bilgilerini alıp geriye token döneceğiz
        Task<CustomResponseDto<TokenModel>> CreateTokenAsync(LoginModel loginModel);
        Task<CustomResponseDto<TokenModel>> CreateTokenByRefreshTokenAsync(string refreshToken);
        // Refresh token sonlandırma veritabanında null çekme
        Task<CustomResponseDto<NoContentDto>> RevokeRefreshTokenAsync(string refreshToken);
    }
}
