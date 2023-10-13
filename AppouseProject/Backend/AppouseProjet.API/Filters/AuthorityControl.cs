using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppouseProjet.API.Filters
{
    public class AuthorityControl : Attribute, IAsyncActionFilter
    {
        // Amaç silme, şifre değişliği vb işlemlerde sadece yetkili kullanıcın ve Admin'in işlem yapabilmesi

        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public AuthorityControl(IUserService userService, IFileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                // Silme yetkisi sadece Admin ve oluşturan kullanıcıda 
                var user = await _userService.GetUserByNameAsync(context.HttpContext.User.Identity.Name);
                int userId = user.Data.Id;
                var file = await _fileService.GetAllByUserIdAsync(userId);

                if (file == null || user.Data.UserType != "Admin")
                {
                    string errorMessage = "Yetkisiz erişim";
                    context.Result = new ObjectResult(CustomResponseDto<NoContentDto>.Failure(401, errorMessage));
                    return;
                };

            }
            catch (Exception ex)
            {

                string errorMessage = $"Kontrol hatası sistem yöneticisine başvurun {ex.Message}";
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(500, errorMessage));
                return;
            }
        }
    }
}
