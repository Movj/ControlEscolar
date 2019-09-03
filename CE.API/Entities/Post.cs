using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Post
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid GrupoId { get; set; }
        public string Mensaje { get; set; }
        public DateTimeOffset FechaPublicacion { get; set; }

        public Grupo Grupo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
