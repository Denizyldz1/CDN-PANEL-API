using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;

namespace AppouseProjet.API.ImageService
{
    public class ImageService : InterfaceImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IQuotaService _quotaService;
        private readonly IFileService _fileService;

        public ImageService(IWebHostEnvironment environment, IUserService userService, IHttpContextAccessor contextAccessor, IQuotaService quotaService, IFileService fileService)
        {
            _environment = environment;
            _userService = userService;
            _contextAccessor = contextAccessor;
            _quotaService = quotaService;
            _fileService = fileService;
        }

        public IEnumerable<FileDto> GetImageList(List<FileDto> dto)
        {
            string HostUrl = "https://localhost:7032";
            foreach (var item in dto)
            {
                string filepath = GetFilePath(item.FileName);
                if (!System.IO.File.Exists(filepath))
                {
                    item.FilePatchUrl = string.Empty;
                }
                else
                {
                    item.FilePatchUrl = HostUrl + "/upload/" + item.FileName;
                }
            }
            return dto;
        }
        public async Task<CustomResponseDto<NoContentDto>> UploadImageAsync()
        {
            var context = _contextAccessor.HttpContext;
            var userName = context.User.Identity.Name;
            if(userName != null) 
            {
                var user = await _userService.GetUserByNameAsync(userName);
                int userId = user.Data.Id;
                if (context.Request.Form.Files.Count>0)
                {
                    var file = context.Request.Form.Files[0];
                    if (file.Length > 5 * 1024 * 1024)
                    {
                        string errorMessage = "Dosya 5MB'tan büyük olamaz";
                        return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);
                    }

                    string uniqueFileName = userId + "_" + file.FileName;
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        string errorMessage = "Bu isimde bir dosya zaten var";
                        return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);
                    }
                    // Kota kontrolü yapılacak
                    var quato = await _quotaService.QuotaByUserId(userId);
                    if (quato.Data.RemainingQuota <= 0)
                    {
                        string errorMessage = "Kullanıcı kotası dolu";
                        return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);

                    }
                    var lastquato = quato.Data.UsedSpaceByte + file.Length;
                    if (lastquato > quato.Data.StorageSpaceByte)
                    {
                        string errorMessage = "Belirtilen dosya boyutu yüklendiği zaman kota aşımı oluyor.";
                        return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);
                    }
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
                        return CustomResponseDto<NoContentDto>.Success(200, "Resim başarılı biri şekilde yüklendi");
                    }
                }
                else
                {
                    string errorMessage = "Yüklenen resim hatası";
                    return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);
                }
            }
            else
            {
                string errorMessage = "Yükleyen kullanıcı bulunamadı sistem yönetecinize başvurun";
                return CustomResponseDto<NoContentDto>.Failure(400, errorMessage);
            }
        }
        public async Task<CustomResponseDto<NoContentDto>> DeleteImageAsync(int imageId) 
        {
            var value = await _fileService.GetById(imageId);
            if(!value.IsSuccess)
            {
                return CustomResponseDto<NoContentDto>.Failure(404,"Resim bulunamadı");
            }
            string imageName = value.Data.FileName;
            string uploadFolder = Path.Combine(_environment.WebRootPath, "Upload");
            string deletedFolder = Path.Combine(uploadFolder, "Deleted");
            // İlgili dosyanın tam yolu oluşturulur
            string filePath = Path.Combine(uploadFolder, imageName);
            // Eğer dosya mevcutsa, Deleted klasörüne taşınır
            if (System.IO.File.Exists(filePath))
            {
                // Deleted klasörü yoksa oluşturulur
                if (!Directory.Exists(deletedFolder))
                {
                    Directory.CreateDirectory(deletedFolder);
                }

                // Dosya, Deleted klasörüne taşınır
                string newFilePath = Path.Combine(deletedFolder, imageName);
                System.IO.File.Move(filePath, newFilePath);

                // Başarılı
                return CustomResponseDto<NoContentDto>.Success(204);
            }
            else
            {
                // Dosya bulunamazsa hata döner
                string errorMessage = "Belirtilen dosya bulunamadı.";

                return CustomResponseDto<NoContentDto>.Failure(400,errorMessage);

            }

        }
        private string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "Upload", fileName);
            return filePath;
        }
        //public CustomResponseDto<NoContentDto> UndoLastOperationAsync(OperationNames operationName)
        //{

        //    if (operationName.ToString() == OperationNames.Delete)
        //    {
        //        return CustomResponseDto<NoContentDto>.Success(200, "Silme işlemi geri alındı");
        //    }
        //    else if (operationName.ToString() == OperationNames.Upload)
        //    {
        //        // Upload işlemiyle ilgili kodları burada gerçekleştirin
        //        // ...
        //        return CustomResponseDto<NoContentDto>.Success(200 ,"Upload işlemi geri alındı.");
        //    }
        //    else
        //    {
        //        return CustomResponseDto<NoContentDto>.Failure(400, "Geçersiz işlem adı. Sadece 'Upload' veya 'Delete' değerlerini kabul ediyoruz.");
        //    }
        //}

        //public class OperationNames
        //{
        //    public const string Upload = "Upload";
        //    public const string Delete = "Delete";
        //}


    }
}
