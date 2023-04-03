using CapaEntidad;
using CapaAccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
