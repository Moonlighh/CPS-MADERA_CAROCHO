﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class entContactForm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombres son obligatorios")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Asunto es obligatorio")]
        public string Asunto { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑ ]{15,}$", ErrorMessage = "El contenido del mensaje no puede tener simbolos y debe tener una logitud minima de 15 caracteres")]
        [Required(ErrorMessage = "Mensaje es obligatorio")]
        public string Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "No se pudo realizar la solicitud")]
        public string IpRemitente { get; set; }

        public entContactForm(string nombre, string email, string asunto, string mensaje, string ipRemitente)
        {
            Nombre = nombre;
            Email = email;
            Asunto = asunto;
            Mensaje = mensaje;
            IpRemitente = ipRemitente;
        }
    }
}