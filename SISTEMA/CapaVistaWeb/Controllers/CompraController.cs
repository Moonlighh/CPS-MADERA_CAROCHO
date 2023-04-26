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
        public ActionResult DetalleCarrito()
        {
            //Lista los productos agregados al carrito
            entUsuario cliente = Session["Usuario"] as entUsuario;
            List<entCarrito> detCompra = logCarrito.Instancia.MostrarDetCarrito(cliente.IdUsuario);
            List<entProducto> listProducto = logProducto.Instancia.ListarProducto();
            List<entProveedor> listProveedor = logProveedor.Instancia.ListarProveedor();
            var lsproducto = new SelectList(listProducto, "idProducto", "nombre");
            var lsproveedor = new SelectList(listProveedor, "idProveedor", "razonSocial");
            ViewBag.ListaProducto = lsproducto;
            ViewBag.ListaProveedor = lsproveedor;
            ViewBag.Lista=detCompra;

            return View(detCompra);
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
                carrito.Producto = prod;
                carrito.Cantidad = pvCantidad;
                carrito.Subtotal = (pvCantidad * detalle.PrecioCompra);
                logCarrito.Instancia.AgregarProductoCarrito(carrito);

                return RedirectToAction("DetalleCarrito");
            }
            catch
            {
                return RedirectToAction("DetalleCarrito");
            }
        }
        public ActionResult EliminarDetalleCarrito(int idProducto)
        {
            try
            {
                //Elimina un producto
                var user = Session["Usuario"] as entUsuario;
                logCarrito.Instancia.EliminarProductoCarrito(idProducto, user.IdUsuario);
                return RedirectToAction("DetalleCarrito");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = e.Message });
            }

        }

        [HttpPost]
        public ActionResult ConfirmarCompra(FormCollection frm)
        {
            try
            {
                entUsuario cliente = Session["Usuario"] as entUsuario;
                //Obtenemos la lista del detalle
                var carrito = logCarrito.Instancia.MostrarDetCarrito(cliente.IdUsuario);

                //Calculamos el total de toda la compra
                double totalCompra = 0;
                for (int i = 0; i < carrito.Count(); i++)
                {
                    totalCompra += carrito[i].Subtotal;
                }

                entCompra compra = new entCompra
                {
                    Usuario = Session["Usuario"] as entUsuario,
                    Estado = true,
                    Total = totalCompra
                };
                //Obtenemos el id de la compra creada
                int idCompra = logCompra.Instancia.CrearCompra(compra);
                compra.IdCompra = idCompra;

                entDetCompra det = new entDetCompra();
                for (int i = 0; i < carrito.Count; i++)
                {
                    det.Producto = carrito[i].Producto;
                    det.Producto = carrito[i].Producto;
                    det.Cantidad = carrito[i].Cantidad;
                    det.Subtotal = carrito[i].Subtotal;
                    det.Compra = compra;
                    logDetCompra.Instancia.CrearDetCompra(det);
                }
                carrito.Clear();
                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Index");

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
                entCarrito carrito = logCarrito.Instancia.MostrarDetCarrito(u.IdUsuario).Where(c => c.IdCarrito == idCarrito).FirstOrDefault();
                return View(carrito);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = e.Message });
            }
        }

        [HttpPost]
        public ActionResult EditarProductoCarrito(entCarrito c, FormCollection frm)
        {
            try
            {

                bool edita = logCarrito.Instancia.EditarProductoCarrito(c);
                if (edita)
                {
                    return RedirectToAction("DetalleCarrito", "Compra");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExceptio = ex.Message });
            }
        }
    }
}