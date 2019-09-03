using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class RolesUsuario
    {
        public Guid RoleId { get; set; }
        public Guid UsuarioId { get; set; }

        public Role Role { get; set; }
        public Usuario Usuario { get; set; }
    }
}
