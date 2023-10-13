using AppouseProject.Core.Dtos.AppUserModels;
using AppouseProject.Core.Dtos.FileDtos;
using AppouseProject.Core.Dtos.QuatoDtos;
using AppouseProject.Core.Entities;
using AutoMapper;

namespace AppouseProject.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<ImageFile, ImageFileDto>().ReverseMap();
            CreateMap<Quota, QuotaDto>().ReverseMap();

            CreateMap<AppUser, SignInModel>().ReverseMap();

        }
    }
}
