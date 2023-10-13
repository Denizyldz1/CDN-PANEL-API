using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Dtos.TokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateToken(LoginModel loginModel)
        {
            var result = await _authenticationService.CreateTokenAsync(loginModel);
            return CreateActionResult(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenModel token)
        {
            var result = await _authenticationService.RevokeRefreshTokenAsync(token.Token);
            return CreateActionResult(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenModel token)
        {
            var result = await _authenticationService.CreateTokenByRefreshTokenAsync(token.Token);
            return CreateActionResult(result);

        }
    }
}
