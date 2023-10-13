using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppouseProjet.API.Filters
{
    public class ImageUpload :Attribute, IAsyncActionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IQuotaService _quotaService;
        private readonly IUserService _userService;

        public ImageUpload(IWebHostEnvironment environment, IQuotaService quotaService, IUserService userService)
        {
            _environment = environment;
            _quotaService = quotaService;
            _userService = userService;
        }        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var user = await _userService.GetUserByNameAsync(context.HttpContext.User.Identity.Name);
                int userId = user.Data.Id;

                if (context.HttpContext.Request.Form.Files.Count > 0)
                {
                    var file = context.HttpContext.Request.Form.Files[0];

                    if (file.Length > 5 * 1024 * 1024)
                    {
                        string errorMessage = "Dosya 5MB'tan büyük olamaz";
                        context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                        return;
                    }

                    string uniqueFileName = userId + "_" + file.FileName;
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        string errorMessage = "Bu isimde bir dosya zaten var";
                        context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                        return;
                    }
                    // Kota kontrolü yapılacak
                    var quato = await _quotaService.QuotaByUserId(userId);
                    if (quato.Data.RemainingQuota <= 0)
                    {
                        string errorMessage = "Kullanıcı kotası dolu";
                        context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                        return;
                    }
                    var lastquato = quato.Data.UsedSpaceByte + file.Length;
                    if (lastquato > quato.Data.StorageSpaceByte)
                    {
                        string errorMessage = "Belirtilen dosya boyutu yüklendiği zaman kota aşımı oluyor.";
                        context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                        return;
                    }

                    //

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(fileStream);
                        fileStream.Flush();
                        //string returnValue = "/Upload/" + uniqueFileName;
                        //context.Result = new OkObjectResult(CustomResponseDto<NoContentDto>.Success(200, returnValue));
                        await next.Invoke();
                        return;
                    }
                }

                else
                {
                    string errorMessage = "Yüklenen resim hatası";
                    context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                    return;
                }

            }
            catch (Exception ex)
            {

                string errorMessage = $"Yükleme hatası sistem yöneticisine başvurun {ex.Message}";
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(500, errorMessage));
                return;
            }

        }
    }

}
