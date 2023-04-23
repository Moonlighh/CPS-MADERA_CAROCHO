using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections;
using System.Collections.Generic;
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
            List<entDetCompra> detCompra = logDetCompra.Instancia.MostrarDetCarrito();
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
                entDetCompra detCompra = new entDetCompra();
                entProducto prod = logProducto.Instancia.BuscarProductoId(Convert.ToInt32(frm["Prd"]));
                entProveedorProducto detalle = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.Producto.IdProducto == prod.IdProducto).FirstOrDefault();
                detCompra.Producto = prod;
                detCompra.Cantidad = pvCantidad;
                detCompra.Subtotal = (pvCantidad * detalle.PrecioCompra);
                logDetCompra.Instancia.AgregarProductoCarrito(detCompra);

                return RedirectToAction("DetalleCarrito");
            }
            catch
            {
                return RedirectToAction("DetalleCarrito");
            }
        }
        public ActionResult EliminarDetalle(int ids)
        {
            try
            {
                //Elimina un producto
                logDetCompra.Instancia.EliminarDetCarrito(ids);
                return RedirectToAction("DetalleCarrito");
            }
            catch
            {
                return RedirectToAction("DetalleCarrito");
            }

        }

        [HttpPost]
        public ActionResult ConfirmarCompra(FormCollection frm)
        {
            try
            {
                //Obtenemos la lista del detalle
                var detalle = logDetCompra.Instancia.MostrarDetCarrito();

                //Calculamos el total de toda la compra
                double total = 0;
                for (int i = 0; i < detalle.Count(); i++)
                {
                    total += detalle[i].Subtotal;
                }

                entProveedor proveedor = logProveedor.Instancia.BuscarIdProveedor(Convert.ToInt32(frm["Prov"]));

                entCompra compra = new entCompra
                {
                    Proveedor = proveedor,
                    Usuario = Session["Usuario"] as entUsuario,
                    Estado = true,
                    Total = total
                };
                //Obtenemos el id de la compra creada
                int idCompra = logCompra.Instancia.CrearCompra(compra);
                compra.IdCompra = idCompra;

                entDetCompra det = new entDetCompra();
                for (int i = 0; i < detalle.Count; i++)
                {
                    det = detalle[i];
                    det.Compra = compra;
                    logDetCompra.Instancia.CrearDetCompra(det);
                }
                detalle.Clear();
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
    }
}