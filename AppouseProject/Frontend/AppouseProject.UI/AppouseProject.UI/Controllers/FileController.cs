using AppouseProject.UI.Models.FileModels;
using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppouseProject.UI.Controllers
{
    public class FileController : Controller
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string token = Request.Cookies["Token"];
                var values = await _fileService.GetAllAsync(token);
                return View(values.Data);
            }
            return RedirectToAction("Index", "Login");

        }
        [Authorize]
        public async Task<IActionResult> ImageList()
        {
            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string token = Request.Cookies["Token"];
                var values = await _fileService.GetAllAsync(token);
                return View(values.Data);
            }
            return RedirectToAction("Index", "Login");
        }
        [Authorize]
        public async Task<IActionResult> UserImageList()
        {
            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                var user = HttpContext.User.Identity;
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                string token = Request.Cookies["Token"];
                var values = await _fileService.GetAllByUserIdAsync(token, userId.Value.ToString());
                return View(values.Data);
            }
            return RedirectToAction("Index", "Login");
        }
        [Authorize]
        public IActionResult Save()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(ImageUploadModel model)
        {
            string uploadedFile = model.File.FileName;

            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string token = Request.Cookies["Token"];
                var values = await _fileService.SaveAsync(model, token);
                if(values.IsSuccess)
                {
                    return View("UserImagelist","File");

                }
                ViewBag.Error = values.Error;
                return View(values.Data);
            }
            return RedirectToAction("Index", "Login");
        }
        [Authorize]
        public async Task<IActionResult> Remove(int id)
        {

            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string token = Request.Cookies["Token"];
                var values = await _fileService.RemoveAsync(token,id);
                if (values.IsSuccess)
                {
                    await UserImageList();
                    return View(nameof(UserImageList));

                }
                await UserImageList();
                return View(nameof(UserImageList));
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
