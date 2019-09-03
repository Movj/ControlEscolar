using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Estudiante
    {
        public Estudiante()
        {
            Calificacion = new HashSet<Calificacion>();
            EstudianteGrupos = new HashSet<EstudianteGrupos>();
        }

        public Guid UsuarioId { get; set; }
        public string Estatus { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Calificacion> Calificacion { get; set; }
        public ICollection<EstudianteGrupos> EstudianteGrupos { get; set; }
    }
}
