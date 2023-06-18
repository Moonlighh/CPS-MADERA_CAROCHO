using CapaAccesoDatos;
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
    // Solo se podrá ingresar a los controladores si estas autorizado
    // y si eres administrador.
    [Authorize]
    [PermisosRol(entRol.Administrador)]
    public class CompraController : Controller
    {
        private readonly ILogCompra _logCompra;

        public CompraController()
        {
            _logCompra = new logCompra(new datCompra());
        }

        #region Compra
        [HttpPost]
        public ActionResult ConfirmarCompra()
        {
            try
            {
                // Obtener el usuario actual
                entUsuario usuario = Session["Usuario"] as entUsuario;

                // Obtener los detalles del carrito de compras del usuario
                var carrito = logCarrito.Instancia.MostrarCarrito(usuario.IdUsuario, null);

                if (carrito.Count == 0)
                {
                    // No hay productos en el carrito, redirigir a la página de error
                    TempData["Error"] = "No tiene productos, por favor asegurese de contar con productos antes de intentar comprar";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    bool exito = _logCompra.CrearCompra(usuario, carrito);
                    if (!exito)
                    {
                        TempData["Error"] = "No se pudo crear la compra";
                        return RedirectToAction("Error", "Home");
                    }
                }
            }

            // Manejo de excepciones
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("DetalleCarrito");
        }

        // Listar todas las compras realizadas
        public ActionResult ComprasRealizadas()
        {
            var compras = new List<entCompra>();
            try
            {
                compras = _logCompra.ListarCompras();
            }
            catch (Exception e)
            {
                TempData[""] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return View(compras);
        }

        public ActionResult DetalleCompra(int idCompra)
        {
            try
            {
                entUsuario u = Session["Usuario"] as entUsuario;
                List<entDetCompra> lista = logDetCompra.Instancia.MostrarDetalleCompra(u.IdUsuario, idCompra);
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

        #region Carrito Compra

        // Este método muestra los productos agregados al carrito de compras del usuario.
        // Toma como parámetro una cadena de caracteres que indica el orden de los productos agregados.
        public ActionResult DetalleCarrito(string orden)
        {
            try
            {
                // Obtiene el usuario actual de la sesión.
                entUsuario cliente = Session["Usuario"] as entUsuario;

                // Obtiene los detalles de compra del usuario actual y los ordena según el parámetro indicado.
                var detCompra = logCarrito.Instancia.MostrarCarrito(cliente.IdUsuario, orden);

                // Agrega los detalles de compra a la ViewBag para que puedan ser accedidos en la vista.
                ViewBag.Lista = detCompra;

                // Obtiene y agrega la cantidad total de productos en el carrito a la ViewBag.
                ViewBag.Cantidad = detCompra.Count;

                // Calcula el total de la compra sumando los subtotales de todos los productos.
                decimal total = detCompra.Sum(detalle => detalle.Subtotal);

                // Agrega el total de la compra a la ViewBag.
                ViewBag.Total = total;

                // Agrega el nombre del usuario a la ViewBag.
                ViewBag.Usuario = cliente.RazonSocial;

                // Muestra la vista con los detalles de la compra.
                return View(detCompra);
            }
            catch (Exception e)
            {
                // En caso de error, guarda el mensaje de error en TempData y redirige a la página de Error.
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // Este método muestra la vista de edición de un producto en el carrito de compras del usuario.
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
        public ActionResult EditarProductoCarrito(entCarrito c)
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
                c.Subtotal = (decimal)(c.Cantidad * detalle.PrecioCompra);

                // Actualizamos la cantidad y subtotal del producto en el carrito
                List<string> errores = new List<string>();
                bool edita = logCarrito.Instancia.EditarProductoCarrito(c, out errores);

                // Redirigimos a la página de error si la edición falló
                if (!edita)
                {
                    TempData["Error"] = "No se pudo editar el producto asegurese de proporcionar datos coherentes: ";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                // Redirigimos a la página de error con el mensaje de la excepción si hubo un error
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("DetalleCarrito", "Compra");

        }

        // Elimina un producto del carrito de compras del usuario actual utilizando el id del proveedor del producto como parámetro.
        public ActionResult EliminarDetalleCarrito(int idProveedorProducto)
        {
            try
            {
                // Obtiene el usuario actual de la sesión.
                var user = Session["Usuario"] as entUsuario;

                // Llama al método EliminarProductoCarrito del objeto logCarrito para eliminar un producto del carrito de compras del usuario.
                logCarrito.Instancia.EliminarProductoCarrito(idProveedorProducto, user.IdUsuario);

                // Redirige a la acción DetalleCarrito para mostrar los productos actualizados en el carrito de compras del usuario.
                return RedirectToAction("DetalleCarrito");
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