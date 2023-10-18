using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProjet.API.Filters;
using AppouseProjet.API.ImageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{
    [Authorize]
    // Yetki kısıtlaması yapmadım her kullanıcı her resmi görsün

    public class FileController : CustomBaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly InterfaceImageService _imageService;

        public FileController(IWebHostEnvironment environment, IFileService fileService, IUserService userService, InterfaceImageService imageService)
        {
            _environment = environment;
            _fileService = fileService;
            _userService = userService;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> PostImage(IFormFile file)
        {
            // Oncelikle image' wwwroot yükleyeceğiz işlem başarılı ise Sql kayıt için göndereceğiz.

            var ımageUploadResult = await _imageService.UploadImageAsync();
            if (ımageUploadResult.StatusCode == 200)
            {
                var user = await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name);
                int userId = user.Data.Id;

                string uniqueFileName = userId + "_" + file.FileName;
                string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
                var fileDto = new FileDto()
                {
                    FileName = uniqueFileName,
                    FileType = "Image",
                    FileSize = (int)file.Length,
                    FilePatchUrl = uploadFolder,
                    UserId = userId
                };
                try
                {
                    var values = await _fileService.AddAsync(fileDto);
                    return CreateActionResult(values);
                }
                catch (Exception)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Failure(500, "Resim yüklendi fakat SQL kaydedilemedi. Hata detayları için yöneticiyle iletişime geçiniz."));
                }
                
            }
            else
            {
                return CreateActionResult(ımageUploadResult);
            }


        }
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> DeleteImage(int id)
        {
           var deleteResult = await _imageService.DeleteImageAsync(id);
            if (deleteResult.StatusCode == 204 || deleteResult.StatusCode == 200)
            {
                try
                {
                    var value = await _fileService.RemoveAsync(id);
                    return CreateActionResult(value);
                }
                catch (Exception ex)
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Failure(500, $"Dosya silindi fakat SQL kaydedilemedi: {ex.Message}"));
                }
            }
            return CreateActionResult(deleteResult);

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fileList = await _fileService.GetAllAsync();
            if (fileList.Errors ==null )
            {
              var lastFileList =   _imageService.GetImageList(fileList.Data.ToList());
              return CreateActionResult(CustomResponseDto<IEnumerable<FileDto>>.Success(200, lastFileList));
            }
            else
            {
                string errorMessage = "Resim yüklenemedi";
                fileList.Errors.Add(errorMessage);
                return CreateActionResult(fileList);
            }
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var fileList = await _fileService.GetAllByUserIdAsync(userId);
            if (fileList.Errors == null)
            {
                var lastFileList = _imageService.GetImageList(fileList.Data.ToList());
                return CreateActionResult(CustomResponseDto<IEnumerable<FileDto>>.Success(200, lastFileList));
            }
            else
            {
                string errorMessage = "Resim yüklenemedi";
                fileList.Errors.Add(errorMessage);
                return CreateActionResult(fileList);
            }

        }


    }
}
