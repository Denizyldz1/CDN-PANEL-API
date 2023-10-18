using AppouseProject.UI.Models.TokenModels;
using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProject.UI.Controllers
{
    public class TokenController : Controller
    {
        private readonly LoginService _loginService;

        public TokenController(LoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {

            if (Request.Cookies["Token"] != null)//Cookie var mı?
            {
                string token = Request.Cookies["Token"];
                var model = new TokenModel() { AccessToken = token };
                return View(model);
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
