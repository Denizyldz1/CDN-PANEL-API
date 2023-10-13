using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.LinkDtos;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{

    public class LinkController : CustomBaseController
    {
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
        {
        };

            return CreateActionResult(CustomResponseDto<IEnumerable<LinkDto>>.Success(200,links));
        }

    }
}
