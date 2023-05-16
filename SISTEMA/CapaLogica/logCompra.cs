using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCompra: ILogCompra
    {
        private readonly IDatCompra _datCompra;

        public logCompra(IDatCompra datCompra)
        {
            _datCompra = datCompra;
        }

        #region Compra
        public bool CrearCompra(entUsuario user, List<entCarrito> listaProductos)
        {
            bool compraCreada = false;
            bool detalleCreado = false;

            try
            {
                // Verificar que el usuario exista, que este activo y que cuente con productos en su carrito.
                if (user != null && user.Activo && listaProductos.Count > 0)
                {
                    /* 
                     * CREAMOS LA COMPRA             
                     */

                    //Calculamos el total de toda la compra
                    decimal totalCompra = listaProductos.Sum(detalle => detalle.Subtotal);

                    // Crear una nueva compra
                    var compra = new entCompra
                    {
                        Usuario = user,
                        Estado = true,
                        Total = totalCompra
                    };

                    // Obtener el ID de la compra creada
                    int idCompra = -1;
                    compraCreada = _datCompra.CrearCompra(compra, out idCompra);
                    /* 
                     * CREAMOS EL DETALLE DE LA COMPRA             
                     */

                    // Verificar que la compra esta creada y que el idGenerado es un valor logico
                    if (compraCreada && idCompra >= 1)
                    {
                        // Creamos un detalle 
                        var detailCompra = new entDetCompra();

                        // Agregar detalles de la compra
                        for (int i = 0; i < listaProductos.Count; i++)
                        {
                            detailCompra.Producto = listaProductos[i].ProveedorProducto.Producto;
                            detailCompra.Cantidad = listaProductos[i].Cantidad;
                            detailCompra.Subtotal = listaProductos[i].Subtotal;

                            detalleCreado = logDetCompra.Instancia.CrearDetCompra(detailCompra, idCompra);

                            // Verificar que cada detalle sea creado correctamente
                            if (detalleCreado == false)
                                throw new Exception("No se pudieron agregar los productos a su compra");
                        }
                    }
                    else
                        throw new Exception("La compra no pudo ser creada, intentelo de nuevo o mas tarde");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return compraCreada && detalleCreado;
        }
        
        public List<entCompra> ListarCompras()
        {
            return _datCompra.ListarCompra();
        }
        
        public bool EliminarCompra(int comp)
        {
            return _datCompra.EliminarCompra(comp);
        }
        #endregion

        #region Otros
        public List<entCompra> BuscarCompra(string busqueda)
        {
            return _datCompra.BuscarCompra(busqueda);
        }
        #endregion
    }
}
