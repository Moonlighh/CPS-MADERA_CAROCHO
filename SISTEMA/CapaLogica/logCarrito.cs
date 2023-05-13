using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        public List<entCarrito> MostrarCarrito(int idUsuario, string orden)
        {
            if (idUsuario <=0)
                return null;
            else
            {
                switch (orden)
                {
                    case "asc": return datCarrito.Instancia.Ordenar(idUsuario, 1);
                    case "desc": return datCarrito.Instancia.Ordenar(idUsuario, 0);
                    default:
                        ;break;
                }
                return datCarrito.Instancia.MostrarCarrito(idUsuario);
            }
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
        public List<entCarrito> OrdenarCarrito(int orden, int idUsuario)
        {
            return datCarrito.Instancia.Ordenar(orden, idUsuario);
        }
        #endregion Carrito de Compras

    }
}
