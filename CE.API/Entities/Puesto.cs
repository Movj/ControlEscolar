using System;
using System.Collections.Generic;

namespace CE.API.Entities
{
    public partial class Puesto
    {
        public Puesto()
        {
            DocentePuestos = new HashSet<DocentePuestos>();
        }

        public Guid Id { get; set; }
        public string NombrePuesto { get; set; }
        public decimal? Salario { get; set; }

        public ICollection<DocentePuestos> DocentePuestos { get; set; }
    }
}
