using AppouseProject.UI.Models;
using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AppouseProject.UI.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly HttpClient _httpClient;

        public LoginController(LoginService loginService, HttpClient httpClient)
        {
            _loginService = loginService;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _loginService.CreateTokenAsync(model);
            if (!token.IsSuccess)
            {
                ViewBag.Errors = token.Errors.First();
                return View();
            }
            var result =  await ContextUserUpdate(model, token.Data.AccessToken);
            if (!result)
            {
                return View();

            }
                return RedirectToAction("Index", "Link");

        }
        private async Task<bool> ContextUserUpdate(LoginModel model, string token)
        {
            try
            {
               var role =  GetRoleFromDecodedToken(token);
                var id = GetRoleIdDecodedToken(token);
                // Daha sonra HttpContext.User'ı güncellemek için erişim tokenını alıyoruz.
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.NameIdentifier,id)   
                // Diğer kullanıcı bilgilerini de ekleyebilirsiniz.
             }, "custom");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);

                Response.Cookies.Append("Token", token, new CookieOptions
                {
                    HttpOnly = true, // Tarayıcı tarafından JavaScript ile erişilemez.
                    Expires = DateTimeOffset.UtcNow.AddMinutes(10) // Cookie süresi (10 dakika).
                });

                await HttpContext.AuthenticateAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string GetRoleFromDecodedToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    // Rol bilgisini token içinden al
                    var role = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                    return role;
                }
                else
                {
                    // Token geçerli değilse veya JWT formatında değilse null döndür
                    return null;
                }
            }
            catch (Exception)
            {
                // Token çözümleme hatası durumunda burada işlem yapabilirsiniz
                // Hata durumunda null döndürebilir veya hata loglama yapabilirsiniz
                return null;
            }
        }
        private string GetRoleIdDecodedToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    // Rol bilgisini token içinden al
                    var id = jsonToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                    return id;

                }
                else
                {
                    // Token geçerli değilse veya JWT formatında değilse null döndür
                    return null;
                }
            }
            catch (Exception)
            {
                // Token çözümleme hatası durumunda burada işlem yapabilirsiniz
                // Hata durumunda null döndürebilir veya hata loglama yapabilirsiniz
                return null;
            }
        }



            public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("MyWepSiteCookie");
            return RedirectToAction("Index", "Login");
        }

    }

}

