using AppouseProject.Core.Entities;

namespace AppouseProject.Core.Abstract.Repositories
{
    public interface IFileRepository
    {
        IQueryable<ImageFile> GetAll();
        IQueryable<ImageFile> GetAllByUserId(int UserId);
        Task<ImageFile> GetById(int id);
        Task AddAsync(ImageFile file);
        Task AddRangeAsync(IEnumerable<ImageFile> files);
        void Remove(ImageFile file);
        void RemoveRange(IEnumerable<ImageFile> files);

    }
}
