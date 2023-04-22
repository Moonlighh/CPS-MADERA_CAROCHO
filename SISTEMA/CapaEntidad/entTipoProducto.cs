using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entTipoProducto
    {
        private int idTipo_producto;
        private string nombre;

        #region Get and Set
        public int IdTipo_producto { get => idTipo_producto; set => idTipo_producto = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        #endregion
    }
}
