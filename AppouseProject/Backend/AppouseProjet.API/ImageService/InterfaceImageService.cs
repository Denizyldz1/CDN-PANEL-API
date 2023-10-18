using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;

namespace AppouseProjet.API.ImageService
{
    public interface InterfaceImageService
    {
        IEnumerable<FileDto> GetImageList(List<FileDto> dto);
        Task<CustomResponseDto<NoContentDto>> UploadImageAsync();
        Task<CustomResponseDto<NoContentDto>> DeleteImageAsync(int imageId);

        


    }
}
