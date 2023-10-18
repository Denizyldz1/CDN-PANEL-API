using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.LinkDtos;
using AppouseProjet.API.MailServices;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{

    public class LinkController : CustomBaseController
    {
        //private readonly IMailService _mailService;

        //public LinkController(IMailService mailService)
        //{
        //    _mailService = mailService;
        //}

        [HttpOptions]
        public IActionResult GetLinks()
        {
            var links = new List<LinkDto>()
            {
                new LinkDto {Name="iisExpress" ,Url="http://localhost:20674"},
                new LinkDto {Name="http" ,Url="http://localhost:5281"},
                new LinkDto {Name="https" ,Url="https://localhost:7032"},
                new LinkDto {Name="https" ,Url="http://localhost:528"}
            };
            return CreateActionResult(CustomResponseDto<IEnumerable<LinkDto>>.Success(200,links));

        }
        //[HttpPost("[action]")]
        //public async Task<IActionResult> SendEmailTest(MailModel model)
        //{
        //    await _mailService.SendEmailAsync(model);
        //    return Ok();
        //}

    }
}
