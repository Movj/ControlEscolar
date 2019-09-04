﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.ModelsDto
{
    public class UsuarioForCreationDto
    {
        //[Required]
        public string Email { get; set; }
        //[Required]
        public string Password { get; set; }
        //[Required]
        public string ApellidoPaterno { get; set; }
       // [Required]
        public string ApellidoMaterno { get; set; }
       // [Required]
        public string Nombre { get; set; }
        //[Required]
        public string TelefonoCelular { get; set; }
        public string TelefonoCasa { get; set; }
        //[Required]
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        //[Required]
        public DateTimeOffset AnioNacimiento { get; set; }
    }
}
