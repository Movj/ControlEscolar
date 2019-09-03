using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            Post = new HashSet<Post>();
            RolesUsuario = new HashSet<RolesUsuario>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoCasa { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public DateTimeOffset AnioNacimiento { get; set; }

        public Docente Docente { get; set; }
        public Estudiante Estudiante { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<RolesUsuario> RolesUsuario { get; set; }
    }
}
