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
        public bool AgregarProductoCarrito(entCarrito carrito)
        {
            bool isValid = ValidationHelper.TryValidateEntity(carrito);
            if (!isValid)
                return false;
           return datCarrito.Instancia.AgregarProductoCarrito(carrito);
        }
        public bool EditarProductoCarrito(entCarrito car)
        {
            bool isValid = ValidationHelper.TryValidateEntity(car);
            if (!isValid)
                return false;
            return datCarrito.Instancia.EditarProductoCarrito(car);
        }
        public bool EliminarProductoCarrito(int idProvProd, int idCliente)
        {
            if (idProvProd <= 0 || idCliente <= 0)
                return false;
            return datCarrito.Instancia.EliminarProductosCarrito(idProvProd, idCliente);
        }
        #endregion Carrito de Compras

    }
}
