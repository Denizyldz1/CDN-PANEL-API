using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Data.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<ImageFile> _dbSet;

        public FileRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ImageFile>();
        }
        public async Task AddAsync(ImageFile file)
        {
            await _dbSet.AddAsync(file);
        }
        public async Task AddRangeAsync(IEnumerable<ImageFile> files)
        {
            await _dbSet.AddRangeAsync(files);
        }

        public IQueryable<ImageFile> GetAll()
        {
            return _dbSet.Where(x=>x.IsDeleted==false)
                .Include(x=>x.AppUser)
                .AsNoTracking().AsQueryable();
        }
        public async Task<ImageFile> GetById(int id)
        {
            var value = await _dbSet.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (value != null)
            {
                return value;
            }
            return default;
        }

        public IQueryable<ImageFile> GetAllByUserId(int UserId)
        {
           var values =  _dbSet.Where(x=>x.UserId==UserId && x.IsDeleted==false)
                .Include(x=>x.AppUser)
                .AsNoTracking().AsQueryable();
            if(values.Any())
            {
                return values;
            }
            return Enumerable.Empty<ImageFile>().AsQueryable();
        }

        public async void Remove (ImageFile file)
        {
            var value = await _dbSet.FindAsync(file.Id);
            value.IsDeleted = true;
        }
        public async void RemoveRange(IEnumerable<ImageFile> files)
        {
            foreach (var item in files)
            {
                var value = await _dbSet.FindAsync(item.Id);
                value.IsDeleted = true;
            }
        }
    }
}
