using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CapaLogica
{
    public class logCarrito
    {
        private static readonly logCarrito _instancia = new logCarrito();

        public static logCarrito Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        // Este metodo valida que solo se registren pedidos hacia un unico proveedor hasta que se finalice la compra
        //private bool PedidoValido(int idUsuario, int idProveedorProducto )
        //{
        //    List<entCarrito> lsCarrito = Instancia.MostrarCarrito(idUsuario, null);
        //    entProveedorProducto detalleProducto = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == idProveedorProducto).SingleOrDefault();

        //    foreach (var item in lsCarrito)
        //    {
        //        if (item.ProveedorProducto.Proveedor.RazonSocial != detalleProducto.Proveedor.RazonSocial)
        //        {
        //            return false;
        //        }

        //    }
        //    return true;

        //}
        public bool AgregarProductoCarrito(entUsuario user, int idProveedorProducto, int pvCantidad)
        {
            bool agregado = false;
            try
            {
                // Validar que el usuario no sea null, que el id del proveedor sea válido y la cantidad sea mayor a 0
                if (user != null && idProveedorProducto >= 1 && pvCantidad >= 1)
                {
                    // Buscar si el producto ya existe en el carrito
                    entCarrito car = Instancia.MostrarCarrito(user.IdUsuario, null).Where(c => c.ProveedorProducto.IdProveedorProducto == idProveedorProducto).SingleOrDefault();
                    if (car != null)
                    {
                        // Si el producto ya existe en el carrito, lanzar una excepción
                        throw new Exception("El producto que intentas agregar ya se encuentra en el carrito de compras");
                    }

                    // Obtener los detalles del producto del proveedor
                    entProveedorProducto detalleProducto = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == idProveedorProducto).SingleOrDefault();

                    // Si el producto existe y el proveedor está activo, crear un objeto entCarrito y agregarlo al carrito
                    entCarrito carrito = new entCarrito();
                    if (detalleProducto != null && detalleProducto.Proveedor.EstProveedor)
                    {
                        // Si el producto existe y el proveedor está activo, modificar los datos del objeto carrito y agregarlo al carrito
                        carrito.Cliente = user;
                        carrito.ProveedorProducto = detalleProducto;
                        carrito.Cantidad = pvCantidad;
                        carrito.Subtotal = (decimal)(pvCantidad * detalleProducto.PrecioCompra);
                        agregado = datCarrito.Instancia.AgregarProductoCarrito(carrito);

                        // Devolver el resultado de la operación (true si se agregó el producto al carrito, false en caso contrario)
                        return agregado;
                    }
                    else
                    {
                        // Si el producto no existe o el proveedor está inactivo, lanzar una excepción
                        throw new Exception("No se puede hacer una compra hacia un proveedor que fue dado de baja o que ya no existe");
                    }
                }
                else
                {
                    // Si el id del proveedor no es válido, el usuario es null, el usuario no es administrador o la cantidad es menor o igual a 0, lanzar una excepción
                    throw new Exception("No cumple con los requisitos para agregar el producto");
                }
            }
            catch (Exception e)
            {
                // Lanzar una excepción si hay un error en el proceso
                throw new Exception("Se producto un error: " + e.Message);
            }
        }

        public bool AgregarProductoCarritoCliente(entUsuario user, int idProveedorProducto, int pvCantidad)
        {
            bool agregado = false;
            try
            {
                // Validar que el usuario no sea null, que el id del proveedor sea válido y la cantidad sea mayor a 0
                if (user != null && idProveedorProducto >= 1 && pvCantidad >= 1)
                {
                    // Buscar si el producto ya existe en el carrito
                    entCarrito car = Instancia.MostrarCarrito(user.IdUsuario, null).Where(c => c.ProveedorProducto.IdProveedorProducto == idProveedorProducto).SingleOrDefault();
                    if (car != null)
                    {
                        // Si el producto ya existe en el carrito, lanzar una excepción
                        throw new Exception("El producto que intentas agregar ya se encuentra en el carrito de compras");
                    }

                    // Obtener los detalles del producto del proveedor
                    entProveedorProducto detalleProducto = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == idProveedorProducto).SingleOrDefault();

                    // Si el producto existe y el proveedor está activo, crear un objeto entCarrito y agregarlo al carrito
                    entCarrito carrito = new entCarrito();
                    if (detalleProducto != null)
                    {
                        // Si el producto existe y el proveedor está activo, modificar los datos del objeto carrito y agregarlo al carrito
                        carrito.Cliente = user;
                        carrito.ProveedorProducto = detalleProducto;
                        carrito.Cantidad = pvCantidad;
                        carrito.Subtotal = (decimal)(pvCantidad * detalleProducto.Producto.PrecioVenta);
                        agregado = datCarrito.Instancia.AgregarProductoCarrito(carrito);

                        // Devolver el resultado de la operación (true si se agregó el producto al carrito, false en caso contrario)
                        return agregado;
                    }
                    throw new Exception("El producto seleccionado no se encuentra disponible");
                }
                else
                {
                    // Si el id del proveedor no es válido, el usuario es null, el usuario no es administrador o la cantidad es menor o igual a 0, lanzar una excepción
                    throw new Exception("No cumple con los requisitos para agregar el producto");
                }
            }
            catch (Exception e)
            {
                // Lanzar una excepción si hay un error en el proceso
                throw new Exception("Se producto un error: " + e.Message);
            }
        }

        public List<entCarrito> MostrarCarrito(int idUsuario, string orden)
        {
            if (idUsuario <= 0)
                return null;
            else
            {
                switch (orden)
                {
                    case "asc": return datCarrito.Instancia.Ordenar(idUsuario, 1);
                    case "desc": return datCarrito.Instancia.Ordenar(idUsuario, 0);
                    default:
                        ; break;
                }
                return datCarrito.Instancia.MostrarCarrito(idUsuario);
            }
        }
        public bool EditarProductoCarrito(entCarrito car, out List<string> errores)
        {
            List<string> strings = new List<string>();
            if (car != null)
            {
                bool isValid = Validation.TryValidateEntityMsj(car, out errores);
                if (!isValid)
                    return false;
            }
            errores = new List<string>();
            return datCarrito.Instancia.EditarProductoCarrito(car);
        }
        public bool EliminarProductoCarrito(int idProvProd, int idCliente)
        {
            if (idProvProd <= 0 || idCliente <= 0)
                return false;
            return datCarrito.Instancia.EliminarProductoCarrito(idProvProd, idCliente);
        }
        #endregion Carrito de Compras

        public List<entCarrito> OrdenarCarrito(int orden, int idUsuario)
        {
            return datCarrito.Instancia.Ordenar(orden, idUsuario);
        }

    }
}
