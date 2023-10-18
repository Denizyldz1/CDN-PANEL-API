using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProject.UI.Controllers
{
    public class QuotaController : Controller
    {
        private readonly   QuotaService _quotaService;
        private readonly UserService _userService;

        public QuotaController(QuotaService quotaService, UserService userService)
        {
            _quotaService = quotaService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string userName = HttpContext.User.Identity.Name;
                string token = Request.Cookies["Token"];
                var values = await _quotaService.GetByUserNameAsync(userName, token);
                return View(values.Data);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
