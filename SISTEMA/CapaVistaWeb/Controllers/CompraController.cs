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
        #region Compra
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
                if (creado && idGenerado!= -1)
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

        // Listar todas las compras realizadas
        public ActionResult Index()
        {
            var compras = logCompra.Instancia.ListarCompra();
            return View(compras);
        }

        public ActionResult DetalleCompra(int idCompra)
        {
            List<entDetCompra> lista = logDetCompra.Instancia.MostrarDetalleCompra(idCompra);
            ViewBag.lista = lista;
            return View(lista);
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
                entCarrito carrito = logCarrito.Instancia.MostrarCarrito(u.IdUsuario, null).Where(c => c.IdCarrito == idCarrito).FirstOrDefault();
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
            catch (Exception e)
            {
                // Redirigimos a la página de error con el mensaje de la excepción si hubo un error
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }

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
                return RedirectToAction("Error", "Home", new { mesjExeption = e.Message });
            }
        }
        #endregion
    }
}