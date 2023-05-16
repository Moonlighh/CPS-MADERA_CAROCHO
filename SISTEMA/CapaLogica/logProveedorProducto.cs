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
        public List<entProveedorProducto> ListarProductoAdmin(string dato, string orden)
        {
            try
            {
                //switch (orden)
                //{
                //    // Si quieren implementar ordenar usar eso
                //    case "asc": return datProveedorProducto.Instancia.Ordenar(1);
                //    case "desc": return datProveedorProducto.Instancia.Ordenar(2);
                //    default:
                //        break;
                //}
                if (string.IsNullOrWhiteSpace(dato))
                {
                    return datProveedorProducto.Instancia.ListarProductoAdmin();
                }
                else
                {
                    return datProveedorProducto.Instancia.BuscarProductoAdmin(dato);
                }
            }
            catch
            {
                throw new Exception("Algo salio mal durante el proceso");
            }
        }

    }
}
