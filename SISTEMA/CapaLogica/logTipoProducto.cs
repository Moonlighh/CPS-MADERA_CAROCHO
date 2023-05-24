using CapaEntidad;
using CapaAccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CapaLogica
{
    public class logTipoProducto
    {
        private static readonly logTipoProducto _instancia = new logTipoProducto();

        public static logTipoProducto Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public bool CrearTipoProducto(entTipoProducto tipoProducto)
        {
            if (tipoProducto == null || string.IsNullOrWhiteSpace(tipoProducto.Tipo))
            {
                return false;
            }
            bool isValid = Regex.IsMatch(tipoProducto.Tipo, @"^[A-Za-z0-9]{10,30}$");

            if (!isValid)
            {
                throw new Exception("El tipo debe tener entre 10 y 30 caracteres (solo letras y números).");
            }
            return datTipoProducto.Instancia.CrearTipoProducto(tipoProducto);
        }
        public List<entTipoProducto> ListarTipoProducto()
        {
            return datTipoProducto.Instancia.ListarTipoProducto();
        }
        public bool ActualizarTipoProducto(entTipoProducto tipoProducto)
        {
            return datTipoProducto.Instancia.ActualizarTipoProducto(tipoProducto);
        }
        public bool EliminarTipoProducto(int id)
        {
            return datTipoProducto.Instancia.EliminarTipoProducto(id);
        }
        
        #endregion CRUD
    }
}
