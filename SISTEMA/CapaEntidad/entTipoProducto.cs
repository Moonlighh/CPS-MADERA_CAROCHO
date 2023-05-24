using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entTipoProducto
    {
        private int idTipo_producto;

        [RegularExpression(@"^[A-Za-z0-9]{10,30}$", ErrorMessage = "Solo se aceptan numeros y letras, máximo 30 caracteres y 10 como minimo.")]
        private string tipo;

        public entTipoProducto() { }

        #region Get and Set
        public int IdTipo_producto { get => idTipo_producto; set => idTipo_producto = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        #endregion
    }
}
