using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.AutoMapperProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Entities.Role, Models.RoleDtoModels.RoleDto>();
            CreateMap<Models.RoleDtoModels.RoleDto, Entities.Role>();
        }
    }
}
