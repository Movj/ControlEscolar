using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Calificacion
    {
        public Guid EstudianteId { get; set; }
        public Guid GrupoId { get; set; }
        public decimal Calificacion1 { get; set; }
        public string Comentarios { get; set; }

        public Estudiante Estudiante { get; set; }
        public Grupo Grupo { get; set; }
    }
}
