using AppouseProject.Core.Dtos.TokenModels;
using AppouseProject.Core.Entities;

namespace AppouseProject.Core.Abstract.Services
{
    public interface ITokenService
    {
        TokenModel CreateToken(AppUser user);
    }
}
