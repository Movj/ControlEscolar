using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class EvaluacionDocente
    {
        public Guid Id { get; set; }
        public string PreguntaEvaluacion { get; set; }
        public decimal Puntaje { get; set; }
        public DateTimeOffset FechaEvaluacion { get; set; }
        public Guid DocenteId { get; set; }

        public Docente Docente { get; set; }
    }
}
