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

        public QuotaController(IQuotaService quotaService)
        {
            _quotaService = quotaService;
        }
        [HttpGet("{userId}")]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> QuotaControl(int userId)
        {
           var value =  await _quotaService.QuotaByUserId(userId);
           return CreateActionResult(value);
        }
    }
}
