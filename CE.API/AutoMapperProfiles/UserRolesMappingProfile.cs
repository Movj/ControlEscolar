using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.AutoMapperProfiles
{
    public class UserRolesMappingProfile : Profile
    {
        public UserRolesMappingProfile()
        {
            CreateMap<Entities.RolesUsuario, Models.UserRoleDto.UserRoleDto>()
                 .ForMember(dest => dest.UserDto, opt => opt.MapFrom(src => src.Usuario))
                 .ForMember(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));

            
        }
    }
}
