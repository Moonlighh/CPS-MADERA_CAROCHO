using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaAccesoDatos;
using System.Text.RegularExpressions;

namespace CapaLogica
{
   public class logTipoEmpleado
    {
        private static readonly logTipoEmpleado _instancia = new logTipoEmpleado();

        public static logTipoEmpleado Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public bool CrearTipoEmpleado(entTipoEmpleado tip)
        {
            if (tip == null || string.IsNullOrWhiteSpace(tip.Nombre))
            {
                return false;
            }

            bool isValid = Regex.IsMatch(tip.Nombre, @"^[a-zA-ZñÑ\s]{10,30}$");
            if (!isValid)
            {
                throw new ArgumentException("El tipo debe tener entre 10 y 30 caracteres (solo letras y espacios).", nameof(tip.Nombre));
            }

            return datTipoEmpleado.Instancia.CrearTipoEmpleado(tip);
        }
        public List<entTipoEmpleado> ListarTipoEmpleado()
        {
            return datTipoEmpleado.Instancia.ListarTipoEmpleado();
        }
        public bool ActualizarTipoEmpleado(entTipoEmpleado tip)
        {
            return datTipoEmpleado.Instancia.ActualizarTipoEmpleado(tip);
        }
        public bool EliminarTipoEmpleado(int id)
        {
            return datTipoEmpleado.Instancia.EliminarTipoEmpleado(id);
        }
        #endregion CRUD
    }
}
