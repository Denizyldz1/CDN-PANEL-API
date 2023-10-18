using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Dtos.QuatoDtos;

namespace AppouseProject.Core.Abstract.Services
{
    public interface IFileService
    {
        Task<CustomResponseDto<IEnumerable<FileDto>>> GetAllAsync();
        Task<CustomResponseDto<IEnumerable<FileDto>>> GetAllByUserIdAsync(int UserId);
        Task<CustomResponseDto<FileDto>> GetById(int Id);
        Task<CustomResponseDto<FileDto>> AddAsync(FileDto dto);
        Task<CustomResponseDto<IEnumerable<FileDto>>> AddRangeAsync(IEnumerable<FileDto> dtos);
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id);
        Task<CustomResponseDto<IEnumerable<NoContentDto>>> RemoveRangeAsync(List<int> ids);

    }
}
