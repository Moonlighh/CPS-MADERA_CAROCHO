using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.Mvc;

namespace MadereraCarocho.Controllers
{
    [Authorize]
    [PermisosRol(entRol.Administrador)]
    public class CompraController : Controller
    {
        public ActionResult Index()
        {
            //Lista todas las compras realizadas
            List<entCompra> lista = logCompra.Instancia.ListarCompra();
            ViewBag.lista = lista;
            return View(lista);
        }
        //Lista los productos agregados al carrito
        public ActionResult DetalleCarrito(string orden)
        {
            try
            {
                entUsuario cliente = Session["Usuario"] as entUsuario;
                var detCompra = logCarrito.Instancia.MostrarCarrito(cliente.IdUsuario, orden);
                ViewBag.Lista = detCompra;
                ViewBag.Cantidad = detCompra.Count;
                // Calcula el total de la compra
                decimal total = detCompra.Sum(detalle => detalle.Subtotal);
                // Agrega el total a la ViewBag
                ViewBag.Total = total;
                ViewBag.Usuario = cliente.RazonSocial;
                return View(detCompra);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = e.Message });
            } 
        }
        [HttpPost]
        public ActionResult AgregarDetCarrito(int pvCantidad, FormCollection frm)
        {
            try
            {
                //Agrega productos al carrito y finalmente manda a listarlos
                entCarrito carrito = new entCarrito();
                entProducto prod = logProducto.Instancia.BuscarProductoId(Convert.ToInt32(frm["Prd"]));
                entProveedorProducto detalle = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.Producto.IdProducto == prod.IdProducto).FirstOrDefault();
                carrito.ProveedorProducto = detalle;
                carrito.Cantidad = pvCantidad;
                carrito.Subtotal = ((decimal)(pvCantidad * detalle.PrecioCompra));
                logCarrito.Instancia.AgregarProductoCarrito(carrito);

                return RedirectToAction("DetalleCarrito");
            }
            catch
            {
                return RedirectToAction("DetalleCarrito");
            }
        }
        public ActionResult EliminarDetalleCarrito(int idProveedorProducto)
        {
            try
            {
                //Elimina un producto
                var user = Session["Usuario"] as entUsuario;
                logCarrito.Instancia.EliminarProductoCarrito(idProveedorProducto, user.IdUsuario);
                return RedirectToAction("DetalleCarrito");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = e.Message });
            }

        }

        [HttpPost]
        public ActionResult ConfirmarCompra()
        {
            try
            {
                // Obtener el cliente actual
                entUsuario cliente = Session["Usuario"] as entUsuario;

                // Obtener los detalles del carrito de compras del cliente
                var carrito = logCarrito.Instancia.MostrarCarrito(cliente.IdUsuario, null);

                if (carrito.Count == 0)
                {
                    // No hay productos en el carrito, redirigir a la página de error
                    TempData["Error"] = "No tiene productos, por favor asegurese de contar con productos antes de intentar comprar";
                    return RedirectToAction("Error", "Home");
                }

                //Calculamos el total de toda la compra
                double totalCompra = (double)carrito.Sum(detalle => detalle.Subtotal);

                // Crear una nueva compra
                var compra = new entCompra
                {
                    Usuario = cliente,
                    Estado = true,
                    Total = (decimal)totalCompra
                };

                // Obtener el ID de la compra creada
                int idGenerado = -1;
                bool creado = logCompra.Instancia.CrearCompra(compra, out idGenerado);
                if (creado)
                {
                    compra.IdCompra = idGenerado;
                    try
                    {
                        var det = new entDetCompra();

                        // Agregar detalles de la compra
                        for (int i = 0; i < carrito.Count; i++)
                        {
                            det.Producto = carrito[i].ProveedorProducto.Producto;
                            det.Cantidad = carrito[i].Cantidad;
                            det.Subtotal = (decimal)carrito[i].Subtotal;
                            det.Compra = compra;
                            logDetCompra.Instancia.CrearDetCompra(det);
                        }
                    }
                    catch (Exception)
                    {
                        // Si no se pueden agregar los detalles de la compra, eliminar la compra creada anteriormente
                        if (!logCompra.Instancia.EliminarCompra(idGenerado))
                        {
                            TempData["Error"] = "No se pudo cancelar la transacción. Comuniquese de inmediato con soporte e informele del problema";
                        }
                        else
                        {
                            TempData["Error"] = "No se pudo agregar los productos a la compra";
                        }
                        return RedirectToAction("Error", "Home");
                    } 
                }
                // Limpiar el carrito de compras del cliente
                carrito.Clear();
                return RedirectToAction("Index");
            }

            // Manejo de excepciones
            catch
            {
                // No se pudo crear la compra, redirigir a la página de error
                TempData["Error"] = "No se pudo crear la compra";
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public ActionResult DetalleCompra(int idCompra)
        {
            List<entDetCompra> lista = logDetCompra.Instancia.MostrarDetalleCompra(idCompra);
            ViewBag.lista = lista;
            return View(lista);
        }
        [HttpGet]
        public ActionResult EditarProductoCarrito(int idCarrito)
        {
            try
            {
                entUsuario u = Session["Usuario"] as entUsuario;
                entCarrito carrito = logCarrito.Instancia.MostrarCarrito(u.IdUsuario, null).Where(c => c.IdCarrito == idCarrito).FirstOrDefault();        
                return View(carrito);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditarProductoCarrito(entCarrito c)
        {
            try
            {
                // Obtenemos el usuario de la sesión actual
                entUsuario u = Session["Usuario"] as entUsuario;

                // Obtenemos el producto del carrito que se va a editar
                entCarrito carrito = logCarrito.Instancia.MostrarCarrito(u.IdUsuario, null).Where(x => x.IdCarrito == c.IdCarrito).FirstOrDefault();

                // Obtenemos los detalles del proveedor del producto
                entProveedorProducto detalle = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == carrito.ProveedorProducto.IdProveedorProducto).FirstOrDefault();

                // Calculamos el subtotal del producto editado
                c.Subtotal = (decimal)(c.Cantidad * detalle.PrecioCompra);

                // Actualizamos la cantidad y subtotal del producto en el carrito
                bool edita = logCarrito.Instancia.EditarProductoCarrito(c);

                // Redirigimos al detalle del carrito si la edición fue exitosa
                if (edita)
                {
                    return RedirectToAction("DetalleCarrito", "Compra");
                }
                else
                {
                    // Redirigimos a la página de error si la edición falló
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (ApplicationException ex)
            {
                // Redirigimos a la página de error con el mensaje de la excepción si hubo un error
                return RedirectToAction("Error", "Home", new { mesjExceptio = ex.Message });
            }

        }
    }
}