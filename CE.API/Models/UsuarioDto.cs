using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.ModelsDto
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        public string TelefonoCelular { get; set; }
        public DateTimeOffset AnioNacimiento { get; set; }
    }
}
