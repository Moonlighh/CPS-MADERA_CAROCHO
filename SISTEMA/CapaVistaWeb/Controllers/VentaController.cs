using CapaAccesoDatos;
using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MadereraCarocho.Controllers
{
    // Solo se podrá ingresar a los controladores si estas autorizado
    // y si eres administrador.
    [Authorize]
    [PermisosRol(entRol.Cliente)]
    public class VentaController : Controller
    {
        private readonly ILogVenta _logVenta;

        public VentaController()
        {
            _logVenta = new logVenta(new datVenta());
        }

        #region Venta
        [HttpPost]
        public ActionResult ConfirmarVenta()
        {
            try
            {
                // Obtener el usuario actual
                entUsuario usuario = Session["Usuario"] as entUsuario;

                // Obtener los detalles del carrito de Ventas del usuario
                var carrito = logCarrito.Instancia.MostrarCarrito(usuario.IdUsuario, null);

                if (carrito.Count == 0)
                {
                    // No hay productos en el carrito, redirigir a la página de error
                    TempData["Error"] = "No tiene productos, por favor asegurese de contar con productos antes de intentar vender";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    bool exito = _logVenta.CrearVenta(usuario, carrito);
                    if (!exito)
                    {
                        TempData["Error"] = "No se pudo crear la venta";
                        return RedirectToAction("Error", "Home");
                    }
                }
            }

            // Manejo de excepciones
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("DetalleCarritoVenta");
        }

        // Listar todas las Ventas realizadas
        public ActionResult VentasRealizadas()
        {
            var Ventas = new List<entVenta>();
            try
            {
                entUsuario usuario = Session["Usuario"] as entUsuario;
                Ventas = _logVenta.ListarVentas(usuario.IdUsuario);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return View(Ventas);
        }

        public ActionResult DetalleVenta(int idVenta)
        {
            try
            {
                entUsuario u = Session["Usuario"] as entUsuario;
                List<entDetVenta> lista = logDetVenta.Instancia.MostrarDetalleVenta(u.IdUsuario, idVenta);
                ViewBag.lista = lista;
                return View(lista);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Carrito Venta

        // Este método muestra los productos agregados al carrito de Ventas del usuario.
        // Toma como parámetro una cadena de caracteres que indica el orden de los productos agregados.
        public ActionResult DetalleCarritoVenta(string orden)
        {
            try
            {
                // Obtiene el usuario actual de la sesión.
                entUsuario cliente = Session["Usuario"] as entUsuario;

                // Obtiene los detalles de Venta del usuario actual y los ordena según el parámetro indicado.
                var detVenta = logCarrito.Instancia.MostrarCarrito(cliente.IdUsuario, orden);

                // Agrega los detalles de Venta a la ViewBag para que puedan ser accedidos en la vista.
                ViewBag.Lista = detVenta;

                // Obtiene y agrega la cantidad total de productos en el carrito a la ViewBag.
                ViewBag.Cantidad = detVenta.Count;

                // Calcula el total de la Venta sumando los subtotales de todos los productos.
                decimal total = detVenta.Sum(detalle => detalle.Subtotal);

                // Agrega el total de la Venta a la ViewBag.
                ViewBag.Total = total;

                // Agrega el nombre del usuario a la ViewBag.
                ViewBag.Usuario = cliente.RazonSocial;

                // Muestra la vista con los detalles de la Venta.
                return View(detVenta);
            }
            catch (Exception e)
            {
                // En caso de error, guarda el mensaje de error en TempData y redirige a la página de Error.
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // Este método muestra la vista de edición de un producto en el carrito de Ventas del usuario.
        // Toma como parámetro el id del producto en el carrito.
        // Recupera el usuario de la sesión y busca el producto en el carrito a partir de su id.
        // Devuelve la vista de edición con el objeto del carrito encontrado.
        // En caso de error, redirige a la acción de error del controlador "Home".
        public ActionResult EditarProductoCarrito(int idCarrito)
        {
            try
            {
                entUsuario u = Session["Usuario"] as entUsuario;
                entCarrito carrito = logCarrito.Instancia.MostrarCarrito(u.IdUsuario, null).Where(c => c.IdCarrito == idCarrito).SingleOrDefault();
                return View(carrito);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditarProductoCarritoVenta(entCarrito c)
        {
            try
            {
                // Obtenemos el usuario de la sesión actual
                entUsuario u = Session["Usuario"] as entUsuario;

                //// Obtenemos el producto del carrito que se va a editar
                entCarrito carrito = logCarrito.Instancia.MostrarCarrito(u.IdUsuario, null).Where(x => x.IdCarrito == c.IdCarrito).SingleOrDefault();

                // Obtenemos los detalles del proveedor del producto
                entProveedorProducto detalle = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == carrito.ProveedorProducto.IdProveedorProducto).SingleOrDefault();

                // Calculamos el subtotal del producto editado
                c.Subtotal = (decimal)(c.Cantidad * detalle.PrecioVenta);

                // Actualizamos la cantidad y subtotal del producto en el carrito
                List<string> errores = new List<string>();
                bool edita = logCarrito.Instancia.EditarProductoCarrito(c, out errores);

                // Redirigimos a la página de error si la edición falló
                if (!edita)
                {
                    TempData["Error"] = "No se pudo editar el producto asegurese de proporcionar datos coherentes: " + errores;
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                // Redirigimos a la página de error con el mensaje de la excepción si hubo un error
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("DetalleCarrito", "Venta");

        }

        // Elimina un producto del carrito de Ventas del usuario actual utilizando el id del proveedor del producto como parámetro.
        public ActionResult EliminarDetalleCarrito(int idProveedorProducto)
        {
            try
            {
                // Obtiene el usuario actual de la sesión.
                var user = Session["Usuario"] as entUsuario;

                // Llama al método EliminarProductoCarrito del objeto logCarrito para eliminar un producto del carrito de Ventas del usuario.
                logCarrito.Instancia.EliminarProductoCarrito(idProveedorProducto, user.IdUsuario);

                // Redirige a la acción DetalleCarrito para mostrar los productos actualizados en el carrito de Ventas del usuario.
                return RedirectToAction("DetalleCarrito");
            }
            catch (Exception e)
            {
                // Si hay algún error, redirige a la acción Error del controlador Home con el mensaje de error.
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult EliminarDetalleCarritoVenta(int idProveedorProducto)
        {
            try
            {
                // Obtiene el usuario actual de la sesión.
                var user = Session["Usuario"] as entUsuario;

                // Llama al método EliminarProductoCarrito del objeto logCarrito para eliminar un producto del carrito de Ventas del usuario.
                logCarrito.Instancia.EliminarProductoCarrito(idProveedorProducto, user.IdUsuario);

                // Redirige a la acción DetalleCarrito para mostrar los productos actualizados en el carrito de Ventas del usuario.
                return RedirectToAction("DetalleCarritoVenta");
            }
            catch (Exception e)
            {
                // Si hay algún error, redirige a la acción Error del controlador Home con el mensaje de error.
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion
    }
}
