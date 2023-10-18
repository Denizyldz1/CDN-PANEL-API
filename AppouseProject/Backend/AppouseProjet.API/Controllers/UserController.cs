using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Enums;
using AppouseProjet.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{

    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] // Kullanıcılar Admin oluşturur
        public async Task<IActionResult> CreateUser([FromBody]SignInModel model)
        {
            if(model.UserType ==UserType.Premium || model.UserType ==UserType.Standart)
            {
                var user = await _userService.CreateUserAsync(model);
                return CreateActionResult(user);
            }
            return CreateActionResult(CustomResponseDto<SignInModel>.Failure(400, "UserType hatası sadece Premium ve Standart değerleri kabul edilir."));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWithRole()
        {
            var users = await _userService.GetAllAsync();
            return CreateActionResult(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return CreateActionResult(user);
        }
        [HttpPatch("[action]")]
        [Authorize]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel model)
        {
            var userName = HttpContext.User.Identity.Name;

            var change = await _userService.ChangeEmailAsync(userName, model.Email);
            return CreateActionResult(change);
        }

        [HttpPatch("[action]")]
        [Authorize]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var userName = HttpContext.User.Identity.Name;
            var change = await _userService.ChangePasswordAsync(userName, model.OldPassword, model.NewPassword);
            return CreateActionResult(change);
        }
        [HttpPatch("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(ChangeRoleModel model)
        {
            var change = await _userService.ChangeRoleAsync(model.UserId.ToString(), model.RoleName);
            return CreateActionResult(change);
        }



    }
}
