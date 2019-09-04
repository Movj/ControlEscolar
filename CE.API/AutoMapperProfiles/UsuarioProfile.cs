using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.AutoMapperProfiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Entities.Usuario, ModelsDto.UsuarioDto>()
                .ForMember(dest => dest.NombreCompleto, opt =>
                opt.MapFrom(src => $"{src.ApellidoPaterno} {src.ApellidoMaterno} {src.Nombre}"));

            CreateMap<Entities.Usuario, ModelsDto.UsuarioFullInfoDto>();

            CreateMap<ModelsDto.UsuarioForUpdateDto, Entities.Usuario>();

            CreateMap<Entities.Usuario, ModelsDto.UsuarioForUpdateDto>();

            CreateMap<ModelsDto.UsuarioForCreationDto, Entities.Usuario>();


        }
    }
}
