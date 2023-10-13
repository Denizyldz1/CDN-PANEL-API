using AppouseProject.Core.Abstract.Repositories;
using AppouseProject.Core.Abstract.Services;
using AppouseProject.Core.Abstract.UnitOfWorks;
using AppouseProject.Core.Dtos;
using AppouseProject.Core.Dtos.QuatoDtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppouseProject.Service.Services
{
    public class QuotaService : IQuotaService
    {
        private readonly IQuotaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QuotaService(IQuotaRepository quotaRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = quotaRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponseDto<IEnumerable<QuotaDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();
            var dtoEntities = new List<QuotaDto>();
            entities.ForEach(e => { dtoEntities.Add(_mapper.Map<QuotaDto>(e)); });
            return CustomResponseDto<IEnumerable<QuotaDto>>.Success(200, dtoEntities);
        }

        public async Task<CustomResponseDto<QuotaDto>> QuotaByUserId(int userId)
        {
            var entity = await _repository.QuotaByUserId(userId);
            var dto = _mapper.Map<QuotaDto>(entity);
            dto.UserId = userId;
            return CustomResponseDto<QuotaDto>.Success(200, dto);
        }

        public async Task<CustomResponseDto<NoContentDto>> QuotaIncrease(int userId, int amount)
        {
             _repository.QuotaIncrease(userId, amount);
             await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);
        }

        public async Task<CustomResponseDto<NoContentDto>> QuotaReduction(int userId, int amount)
        {
            _repository.QuotaReduction(userId, amount);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);
        }
        public async Task<CustomResponseDto<NoContentDto>> QuotaUpdate(int userId, int amount,string userType)
        {
            _repository.QuotaUpdate(userId, amount, userType);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);

        }
    }
}
