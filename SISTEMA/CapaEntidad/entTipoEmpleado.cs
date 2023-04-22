using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entTipoEmpleado
    {
        private int idTipo_Empleado;
        private string nombre;

        #region Get and Set
        public int IdTipo_Empleado { get => idTipo_Empleado; set => idTipo_Empleado = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        #endregion
    }
}
