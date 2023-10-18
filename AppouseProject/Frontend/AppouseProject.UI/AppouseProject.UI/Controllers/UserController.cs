using AppouseProject.UI.Models;
using AppouseProject.UI.Models.UserModels;
using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using System.Reflection;

namespace AppouseProject.UI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Token"] != null)
            {
                string token = Request.Cookies["Token"];
                var result = await _userService.GetAllAsync(token);
                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
                return RedirectToAction("ImageList", "File");
            }
            return RedirectToAction("ImageList", "File");

        }
        [Authorize(Roles ="Admin")]
        public IActionResult Save()
        {
            RoleFill();
            return View();
        }

        private void RoleFill()
        {
            var roleList = new List<string>()
            {
                UserRoleType.Standart,
                UserRoleType.Premium
            };
            ViewBag.UserType = new SelectList(roleList);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SignUpModel model)
        {
            if(ModelState.IsValid)
            {
               if(Request.Cookies["Token"] != null)
                {
                    string token = Request.Cookies["Token"];
                    var result = await _userService.SaveAsync(model,token);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ViewBag.Error = result.Error;
                    RoleFill();
                    return View(model);
                }
                ViewBag.Error = "Cookie hatası";
                RoleFill();
                return View(model);
            }
            RoleFill();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRole(int userId,string roleName)
        {
            if (Request.Cookies["Token"] != null)
            {
                string token = Request.Cookies["Token"];
                var model = new UserRoleModel()
                {
                    UserId = userId,
                    RoleName = roleName
                };
                var result = await _userService.ChangeRoleAsync(token, model);
                if (result.IsSuccess)
                {
                   await Index();
                   return View(nameof(Index));
                }
                await Index();
                return View(nameof(Index));
            }
            return RedirectToAction("Index", "Login");

        }
    }
}
