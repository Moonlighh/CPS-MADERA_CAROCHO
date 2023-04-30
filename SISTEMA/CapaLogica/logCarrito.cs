using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCarrito
    {
        private static readonly logCarrito _instancia = new logCarrito();

        public static logCarrito Instancia
        {
            get { return _instancia; }
        }

        #region Carrito de Compras
        public void AgregarProductoCarrito(entCarrito det)
        {
           datCarrito.Instancia.AgregarProductoCarrito(det);
        }
        public List<entCarrito> MostrarDetCarrito(int idUsuario)
        {
            try
            {
                return datCarrito.Instancia.MostrarDetCarrito(idUsuario);
            }
            catch {
                throw new ApplicationException ("No se pudo mostrar sus productos");
            }
        }
        public bool EditarProductoCarrito(entCarrito car)
        {
            logProveedorProducto obj = new logProveedorProducto();
            //double subTotal 

            return datCarrito.Instancia.EditarProductoCarrito(car);
        }
        public bool EliminarProductoCarrito(int idProvProd, int idCliente)
        {
            return datCarrito.Instancia.EliminarProductoCarrito(idProvProd, idCliente);
        }
        #endregion Carrito de Compras

    }
}
