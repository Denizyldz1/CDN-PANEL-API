using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Dtos.TokenModels;
using AppouseProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationRepository _authRepository;

        public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService, IUnitOfWork unitOfWork, IAuthenticationRepository authRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _authRepository = authRepository;
        }

        public async Task<CustomResponseDto<TokenModel>> CreateTokenAsync(LoginModel loginModel)
        {
            if (loginModel == null) throw new ArgumentNullException(nameof(loginModel));
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user == null) if (user == null) return CustomResponseDto<TokenModel>.Failure(400, "Email or Password is wrong");

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password)) // False ise
            {
                return CustomResponseDto<TokenModel>.Failure(400, "Email or Password is wrong");
            }
            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _authRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _authRepository.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration
                });
            }
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<TokenModel>.Success(200, token);
        }

        public async Task<CustomResponseDto<TokenModel>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _authRepository.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return CustomResponseDto<TokenModel>.Failure(404, "Refresh token not found");
            }
            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId.ToString());
            if (user == null)
            {
                return CustomResponseDto<TokenModel>.Failure(404, "UserId not found");

            }

            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.RefreshToken = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();
            return CustomResponseDto<TokenModel>.Success(200, tokenDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _authRepository.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
            {
                return CustomResponseDto<NoContentDto>.Failure(404, "Refresh token not found");

            }
            _authRepository.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}
