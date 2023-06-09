using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logVenta : ILogVenta
    {
        private readonly IDatVenta _datVenta;

        public logVenta(IDatVenta datVenta)
        {
            _datVenta = datVenta;
        }

        #region Venta
        public bool CrearVenta(entUsuario user, List<entCarrito> listaProductos)
        {
            bool VentaCreada = false;
            bool detalleCreado = false;

            try
            {
                // Verificar que el usuario exista, que este activo y que cuente con productos en su carrito.
                if (user != null && user.Activo && listaProductos.Count > 0)
                {
                    /* 
                     * CREAMOS LA Venta             
                     */

                    //Calculamos el total de toda la Venta
                    decimal totalVenta = listaProductos.Sum(detalle => detalle.Subtotal);

                    // Crear una nueva Venta
                    var Venta = new entVenta
                    {
                        Usuario = user,
                        Estado = true,
                        Total = totalVenta
                    };

                    // Obtener el ID de la Venta creada
                    int idVenta = -1;
                    VentaCreada = _datVenta.CrearVenta(Venta, out idVenta);
                    /* 
                     * CREAMOS EL DETALLE DE LA Venta             
                     */

                    // Verificar que la Venta esta creada y que el idGenerado es un valor logico
                    if (VentaCreada && idVenta >= 1)
                    {
                        // Creamos un detalle 
                        var detailVenta = new entDetVenta();

                        // Agregar detalles de la Venta
                        for (int i = 0; i < listaProductos.Count; i++)
                        {
                            detailVenta.Producto = listaProductos[i].ProveedorProducto.Producto;
                            detailVenta.Cantidad = listaProductos[i].Cantidad;
                            detailVenta.Subtotal = listaProductos[i].Subtotal;

                            detalleCreado = logDetVenta.Instancia.CrearDetVenta(detailVenta, idVenta);

                            // Verificar que cada detalle sea creado correctamente
                            if (detalleCreado == false)
                                throw new Exception("No se pudieron agregar los productos a su Venta");
                        }
                    }
                    else
                        throw new Exception("La Venta no pudo ser creada, intentelo de nuevo o mas tarde");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return VentaCreada && detalleCreado;
        }

        public List<entVenta> ListarVentas(int idCliente)
        {
            return _datVenta.ListarVenta(idCliente);
        }

        
        #endregion

        #region Otros
        public List<entVenta> BuscarVenta(string busqueda)
        {
            return _datVenta.BuscarVenta(busqueda);
        }
        #endregion
    }
}
