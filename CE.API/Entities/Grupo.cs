using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Grupo
    {
        public Grupo()
        {
            Calificacion = new HashSet<Calificacion>();
            EstudianteGrupos = new HashSet<EstudianteGrupos>();
            Post = new HashSet<Post>();
        }

        public Guid Id { get; set; }
        public string NivelGrupo { get; set; }
        public DateTimeOffset? InicioCurso { get; set; }
        public DateTimeOffset? FinCurso { get; set; }
        public int? CapacidadMaxima { get; set; }
        public string TurnoCurso { get; set; }
        public Guid DocenteId { get; set; }

        public Docente Docente { get; set; }
        public ICollection<Calificacion> Calificacion { get; set; }
        public ICollection<EstudianteGrupos> EstudianteGrupos { get; set; }
        public ICollection<Post> Post { get; set; }
    }
}
