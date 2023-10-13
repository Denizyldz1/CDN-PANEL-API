using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Enums;
using AppouseProject.Service.Services;
using AppouseProjet.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppouseProjet.API.Controllers
{
    [Authorize]
    // Yetki kısıtlaması yapmadım her kullanıcı her resmi görsün

    public class ImageController : CustomBaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public ImageController(IWebHostEnvironment environment, IFileService fileService, IUserService userService)
        {
            _environment = environment;
            _fileService = fileService;
            _userService = userService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ImageUpload))]
        public async Task<IActionResult> PostImage(IFormFile file)
        {
            var user = await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name);
            int userId = user.Data.Id;

            string uniqueFileName = userId + "_" + file.FileName;
            string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
            var fileDto = new ImageFileDto()
            {
                FileName = uniqueFileName,
                FileType = "Image",
                FileSize = (int)file.Length,
                FilePatchUrl = uploadFolder,
                UserId = userId
            };
            try
            {
              var values =   await _fileService.AddAsync(fileDto);
              return  CreateActionResult(values);
            }
            catch (Exception)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Failure(500, "Resim yüklendi fakat SQL kaydedilemedi. Hata detayları için yöneticiyle iletişime geçiniz."));
            }
        }
        [HttpDelete]
        [ServiceFilter(typeof(ImageDelete))]
        [ServiceFilter(typeof(AuthorityControl))]
        public async Task<IActionResult> DeleteImage(int id)
        {

            try
            {
              var value =   await _fileService.RemoveAsync(id);
                return CreateActionResult(value);
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun mesajla hata döner
                return CreateActionResult(CustomResponseDto<NoContentDto>.Failure(500, $"Dosya silindi fakat SQL kaydedilemedi: {ex.Message}"));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _fileService.GetAllAsync();
            return CreateActionResult(values);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var values = await _fileService.GetAllByUserIdAsync(userId);
            return CreateActionResult(values);

        }


    }
}
