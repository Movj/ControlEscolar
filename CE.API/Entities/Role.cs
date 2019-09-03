using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Role
    {
        public Role()
        {
            RolesUsuario = new HashSet<RolesUsuario>();
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<RolesUsuario> RolesUsuario { get; set; }
    }
}
