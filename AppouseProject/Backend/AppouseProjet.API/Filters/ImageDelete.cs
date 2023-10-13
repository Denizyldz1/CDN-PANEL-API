using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppouseProjet.API.Filters
{
    public class ImageDelete : Attribute, IAsyncActionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        public ImageDelete(IWebHostEnvironment environment, IFileService fileService)
        {
            _environment = environment;
            _fileService = fileService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {


            try
            {

                var Id = context.ActionArguments.Values.FirstOrDefault();
                var fileId =(int)Id;
               var value = await _fileService.GetById(fileId);
                if(value.Data == null)
                {
                    context.Result = new ObjectResult(value);
                    return;
                }
               string ImageName = value.Data.FileName;

                string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
                string deletedFolder = Path.Combine(uploadFolder, "Deleted");

                // İlgili dosyanın tam yolu oluşturulur
                string filePath = Path.Combine(uploadFolder, ImageName);

                // Eğer dosya mevcutsa, Deleted klasörüne taşınır
                if (System.IO.File.Exists(filePath))
                {
                    // Deleted klasörü yoksa oluşturulur
                    if (!Directory.Exists(deletedFolder))
                    {
                        Directory.CreateDirectory(deletedFolder);
                    }

                    // Dosya, Deleted klasörüne taşınır
                    string newFilePath = Path.Combine(deletedFolder, ImageName);
                    System.IO.File.Move(filePath, newFilePath);

                    // Başarılı

                    await next.Invoke();
                    return;
                }
                else
                {
                    // Dosya bulunamazsa hata döner
                    string errorMessage = "Belirtilen dosya bulunamadı.";
                    context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(400, errorMessage));
                    return;
                }

            }
            catch (Exception ex)
            {
                string errorMessage = $"Silme hatası sistem yöneticisine başvurun {ex.Message}";
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Failure(500, errorMessage));
                return;
            }
        }
    }
}
