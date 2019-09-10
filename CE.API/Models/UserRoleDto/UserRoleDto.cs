using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Models.UserRoleDto
{
    public class UserRoleDto
    {
        public ModelsDto.UsuarioDto UserDto { get; set; }
        public Models.RoleDtoModels.RoleDto RoleDto { get; set; }
    }
}
