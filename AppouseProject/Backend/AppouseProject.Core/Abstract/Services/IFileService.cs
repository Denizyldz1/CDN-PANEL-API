using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Dtos.QuatoDtos;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IFileService
    {
        Task<CustomResponseDto<IEnumerable<ImageFileDto>>> GetAllAsync();
        Task<CustomResponseDto<IEnumerable<ImageFileDto>>> GetAllByUserIdAsync(int UserId);
        Task<CustomResponseDto<ImageFileDto>> GetById(int Id);
        Task<CustomResponseDto<ImageFileDto>> AddAsync(ImageFileDto dto);
        Task<CustomResponseDto<IEnumerable<ImageFileDto>>> AddRangeAsync(IEnumerable<ImageFileDto> dtos);
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id);
        Task<CustomResponseDto<IEnumerable<NoContentDto>>> RemoveRangeAsync(List<int> ids);

    }
}
