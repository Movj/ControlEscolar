using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Docente
    {
        public Docente()
        {
            DocentePuestos = new HashSet<DocentePuestos>();
            EvaluacionDocente = new HashSet<EvaluacionDocente>();
            Grupo = new HashSet<Grupo>();
        }

        public Guid UsuarioId { get; set; }
        public DateTimeOffset? Antiguedad { get; set; }
        public decimal? Evaluacion { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<DocentePuestos> DocentePuestos { get; set; }
        public ICollection<EvaluacionDocente> EvaluacionDocente { get; set; }
        public ICollection<Grupo> Grupo { get; set; }
    }
}
