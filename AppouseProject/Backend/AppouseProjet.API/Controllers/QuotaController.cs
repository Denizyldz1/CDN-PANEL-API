using AppouseProject.Core.Abstract.Services;
using AppouseProjet.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{
    [Authorize]
    public class QuotaController : CustomBaseController
    {
        private readonly IQuotaService _quotaService;
        private readonly IUserService _userService;

        public QuotaController(IQuotaService quotaService, IUserService userService)
        {
            _quotaService = quotaService;
            _userService = userService;
        }
        [HttpGet("{userName}")]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> QuotaControl(string userName)
        {
           var user =  await _userService.GetUserByNameAsync(userName);
           var value =  await _quotaService.QuotaByUserId(user.Data.Id);         
           return CreateActionResult(value);
        }
    }
}
