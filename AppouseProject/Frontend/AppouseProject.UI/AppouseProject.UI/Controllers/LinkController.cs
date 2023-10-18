using AppouseProject.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProject.UI.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        private readonly LinkService _linkService;

        public LinkController(LinkService linkService)
        {
            _linkService = linkService;
        }

        public async Task<IActionResult> Index()
        {
                var values = await _linkService.GetAllAsync();
                return View(values);


        }
    }
}
