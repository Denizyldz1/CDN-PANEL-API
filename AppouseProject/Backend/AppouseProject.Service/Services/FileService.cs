using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotaRepository _quotaRepository;

        public FileService(IFileRepository repository, IMapper mapper, IUnitOfWork unitOfWork , IQuotaRepository quotaRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _quotaRepository = quotaRepository;
        }
        public async Task<CustomResponseDto<FileDto>> AddAsync(FileDto dto)
        {
                var entity = new ImageFile()
                {
                    FileName = dto.FileName,
                    FileType = dto.FileType,
                    FileSize = dto.FileSize,
                    FilePatchUrl = dto.FilePatchUrl,
                    UserId = dto.UserId,
                };
                await _repository.AddAsync(entity);

                _quotaRepository.QuotaIncrease(dto.UserId, dto.FileSize);
                await _unitOfWork.CommitAsync();

                dto.Id = entity.Id;
                dto.CreatedDate = entity.CreatedDate;
                dto.IsDeleted = entity.IsDeleted;
                return CustomResponseDto<FileDto>.Success(201, dto);
        }
        public async Task<CustomResponseDto<IEnumerable<FileDto>>> AddRangeAsync(IEnumerable<FileDto> dtos)
        {
            var addedFiles = new List<ImageFile>();

                foreach (var dto in dtos)
                {
                    var entity = new ImageFile()
                    {
                        FileName = dto.FileName,
                        FileType = dto.FileType,
                        FileSize = dto.FileSize,
                        FilePatchUrl = dto.FilePatchUrl,
                        UserId = dto.UserId,
                    };
                    addedFiles.Add(entity);
                    _quotaRepository.QuotaIncrease(dto.UserId, dto.FileSize);
                }
                await _repository.AddRangeAsync(addedFiles);
                await _unitOfWork.CommitAsync();
                return CustomResponseDto<IEnumerable<FileDto>>.Success(201,dtos);
        }

        public async Task<CustomResponseDto<IEnumerable<FileDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();
            var dtoEntities = new List<FileDto>();

            //entities.ForEach(e => { dtoEntities.Add(_mapper.Map<ImageFileDto>(e)); }); //Include UserName görmüyor
            foreach (var entity in entities)
            {
               var map =  _mapper.Map<FileDto>(entity);
                map.UserName = entity.AppUser.UserName;
                dtoEntities.Add(map);
            }

            return CustomResponseDto<IEnumerable<FileDto>>.Success(200, dtoEntities);
        }

        public async Task<CustomResponseDto<IEnumerable<FileDto>>> GetAllByUserIdAsync(int UserId)
        {
           var entities = await _repository.GetAllByUserId(UserId).ToListAsync();
           var dtoEntities = new List<FileDto>();
            foreach (var entity in entities)
            {
                var map = _mapper.Map<FileDto>(entity);
                map.UserName = entity.AppUser.UserName;
                dtoEntities.Add(map);
            }
            return CustomResponseDto<IEnumerable<FileDto>>.Success(200, dtoEntities);
        }

        public async Task<CustomResponseDto<FileDto>> GetById(int Id)
        {

               var value =  await _repository.GetById(Id);
               if(value == null)
                {
                    return CustomResponseDto<FileDto>.Failure(404, " Dosya bulunamadı");
                }
               var dto = _mapper.Map<FileDto>(value);
                return CustomResponseDto<FileDto>.Success(200, dto);
        }

        public async  Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {         

                var value = await _repository.GetById(id);
                _repository.Remove(value);
                _quotaRepository.QuotaReduction(value.UserId, value.FileSize);
                await _unitOfWork.CommitAsync();

                return CustomResponseDto<NoContentDto>.Success(204);

        }

        public async Task<CustomResponseDto<IEnumerable<NoContentDto>>> RemoveRangeAsync(List<int> ids)
        {
            var removeFiles = new List<ImageFile>();

                foreach (var id in ids)
                {
                    var value = await _repository.GetById(id);
                    removeFiles.Add(value);
                    _quotaRepository.QuotaReduction(value.UserId, value.FileSize);
                }               
                _repository.RemoveRange(removeFiles);
                await _unitOfWork.CommitAsync();

                return CustomResponseDto<IEnumerable<NoContentDto>>.Success(204);

        }
    }
}
