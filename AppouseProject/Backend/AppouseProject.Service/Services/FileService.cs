using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AppouseProject.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotaRepository _quotaRepository;

        public FileService(IFileRepository repository, IMapper mapper, IUnitOfWork unitOfWork = null, IQuotaRepository quotaRepository = null)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _quotaRepository = quotaRepository;
        }

        public async Task<CustomResponseDto<ImageFileDto>> AddAsync(ImageFileDto dto)
        {
            try
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
                return CustomResponseDto<ImageFileDto>.Success(201, dto);

            }
            catch (Exception ex)
            {

                return CustomResponseDto<ImageFileDto>.Failure(500, $"Image kayıt hatası - {ex.Message}");
            }
        }

        public async Task<CustomResponseDto<IEnumerable<ImageFileDto>>> AddRangeAsync(IEnumerable<ImageFileDto> dtos)
        {
            var addedFiles = new List<ImageFile>();

            try
            {
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
                return CustomResponseDto<IEnumerable<ImageFileDto>>.Success(201,dtos);
            }
            catch (Exception ex)
            {
                return CustomResponseDto<IEnumerable<ImageFileDto>>.Failure(500, $"Toplu dosya ekleme hatası - {ex.Message}");
            }
        }

        public async Task<CustomResponseDto<IEnumerable<ImageFileDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();
            var dtoEntities = new List<ImageFileDto>();

            //entities.ForEach(e => { dtoEntities.Add(_mapper.Map<ImageFileDto>(e)); }); //Include UserName görmüyor
            foreach (var entity in entities)
            {
               var map =  _mapper.Map<ImageFileDto>(entity);
                map.UserName = entity.AppUser.UserName;
                dtoEntities.Add(map);
            }

            return CustomResponseDto<IEnumerable<ImageFileDto>>.Success(200, dtoEntities);
        }

        public async Task<CustomResponseDto<IEnumerable<ImageFileDto>>> GetAllByUserIdAsync(int UserId)
        {
           var entities = await _repository.GetAllByUserId(UserId).ToListAsync();
           var dtoEntities = new List<ImageFileDto>();
            foreach (var entity in entities)
            {
                var map = _mapper.Map<ImageFileDto>(entity);
                map.UserName = entity.AppUser.UserName;
                dtoEntities.Add(map);
            }
            return CustomResponseDto<IEnumerable<ImageFileDto>>.Success(200, dtoEntities);
        }

        public async Task<CustomResponseDto<ImageFileDto>> GetById(int Id)
        {
            try
            {
               var value =  await _repository.GetById(Id);
               if(value == null)
                {
                    return CustomResponseDto<ImageFileDto>.Failure(404, " Dosya bulunamadı");
                }
               var dto = _mapper.Map<ImageFileDto>(value);
                return CustomResponseDto<ImageFileDto>.Success(200, dto);

            }
            catch (Exception ex)
            {

                return CustomResponseDto<ImageFileDto>.Failure(500, $" {ex.Message}");

            }
        }

        public async  Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {         
            try
            {
                var value = await _repository.GetById(id);
                _repository.Remove(value);
                _quotaRepository.QuotaReduction(value.UserId, value.FileSize);
                await _unitOfWork.CommitAsync();

                return CustomResponseDto<NoContentDto>.Success(204);

            }
            catch (Exception ex)
            {

                return CustomResponseDto<NoContentDto>.Failure(500, $"Image silme hatası - {ex.Message}");

            }
        }

        public async Task<CustomResponseDto<IEnumerable<NoContentDto>>> RemoveRangeAsync(List<int> ids)
        {
            var removeFiles = new List<ImageFile>();

            try
            {
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
            catch (Exception ex)
            {

                return CustomResponseDto<IEnumerable<NoContentDto>>.Failure(500, $"Image silme hatası - {ex.Message}");

            }

        }
    }
}
