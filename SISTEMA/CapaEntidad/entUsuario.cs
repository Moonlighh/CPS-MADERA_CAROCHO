using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class entUsuario
    {
        /*Para poder usar el DataAnnotations es necesario descargar el paquete nuget -> System.ComponentModel.Annotations en la capa que deseas usar*/

        [Display(Name = "ID")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "La razón social es requerida.")]
        //[StringLength(40, MinimumLength = 10, ErrorMessage = "La razón social debe tener entre 10 y 40 caracteres.")]
        //[RegularExpression("^[^0-9]*$", ErrorMessage = "La razón social no puede contener números.")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El DNI es requerido.")]
        [RegularExpression(@"^\d{8}[a-zA-Z]?$", ErrorMessage = "El DNI debe tener 8 dígitos y una letra opcional.")]
        public string Dni { get; set; }

        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos.")]
        public string Telefono { get; set; }

        [StringLength(60, ErrorMessage = "La dirección debe tener como máximo 60 caracteres.")]
        public string Direccion { get; set; }

        public entUbigeo Ubigeo { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }

        [RegularExpression("^[a-zA-Z0-9._-]{3,20}$", ErrorMessage = "El nombre de usuario debe tener entre 3 y 20 caracteres y solo puede contener letras, números y algunos caracteres especiales.")]
        public string UserName { get; set; }

        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",
        //ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula y un número")]
        public string Pass { get; set; }

        public entRol Rol { get; set; }

        public entRoll Roll { get; set; }

        public bool Activo { get; set; }
    }
}
