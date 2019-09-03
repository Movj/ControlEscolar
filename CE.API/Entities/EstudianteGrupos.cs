using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class EstudianteGrupos
    {
        public Guid EstudianteId { get; set; }
        public Guid GrupoId { get; set; }
        public DateTimeOffset FechaInscripcion { get; set; }

        public Estudiante Estudiante { get; set; }
        public Grupo Grupo { get; set; }
    }
}
