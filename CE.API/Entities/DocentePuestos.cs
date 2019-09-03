using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class DocentePuestos
    {
        public Guid DocenteId { get; set; }
        public Guid PuestoId { get; set; }

        public Docente Docente { get; set; }
        public Puesto Puesto { get; set; }
    }
}
