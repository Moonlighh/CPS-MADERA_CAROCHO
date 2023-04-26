using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logProveedorProducto
    {
        private static logProveedorProducto _instancia = new logProveedorProducto();

        public static logProveedorProducto Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public List<entProveedorProducto> ListarProveedorProducto()
        {
            return datProveedorProducto.Instancia.ListarProveedorProducto();
        }
        #endregion
        public List<entProveedorProducto> ListarProductoAdmin()
        {
            return datProveedorProducto.Instancia.ListarProductoAdmin();
        }
        public List<entProveedorProducto> BuscarProductoAdmin(string dato)
        {
            return datProveedorProducto.Instancia.BuscarProductoAdmin(dato);
        }

    }
}
