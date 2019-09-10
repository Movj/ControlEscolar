using CE.API.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Models
{
    public class UsuarioRolesDto 
    {
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        public string TelefonoCelular { get; set; }
        public DateTimeOffset AnioNacimiento { get; set; }
        public List<RoleDtoModels.RoleDto> Roles { get; set; } = new List<RoleDtoModels.RoleDto>();
    }
}
