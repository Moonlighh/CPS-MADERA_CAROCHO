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
    [Authorize]
    [PermisosRol(entRol.Cliente)]
    public class VentaController : Controller
    {
        public ActionResult Listar()
        {
            //Lista todas las compras realizadas por un cliente
            entUsuario usu = Session["Usuario"] as entUsuario;
            List<entVenta> lista = logVenta.Instancia.ListarVenta(usu.IdUsuario);
            ViewBag.lista = lista;
            return View(lista);
        }
        public ActionResult Detalle()
        {
            //Lista los productos agregados al carrito
            List<entDetVenta> detalle = logDetVenta.Instancia.Mostrardetventa();
            List<entProducto> listaproducto = logProducto.Instancia.ListarProductos(null, null);
            var lsproducto = new SelectList(listaproducto, "idProducto", "nombre");
            ViewBag.listaproducto = lsproducto;
            return View(detalle);
        }
        [HttpPost]
        public ActionResult LlenarDetalle(int pCantidad, FormCollection frm)
        {
            try
            {
                //Agrega productos al carrito y finalmente manda a listarlos
                entDetVenta vn = new entDetVenta();
                entProducto prod = logProducto.Instancia.BuscarProductoId(Convert.ToInt32(frm["pProd"]));
                vn.Producto = prod;
                vn.Cantidad = pCantidad;
                vn.SubTotal = (pCantidad * prod.PrecioVenta);
                logDetVenta.Instancia.LlenarDetventa(vn);
                return RedirectToAction("Detalle");
            }
            catch
            {
                return RedirectToAction("Detalle");
            }
        }
        public ActionResult EliminarDetalle(int ids)
        {
            try
            {
                //Elimina un producto
                logDetVenta.Instancia.EliminarDetalle(ids);
                return RedirectToAction("Detalle");
            }
            catch
            {
                return RedirectToAction("Detalle");
            }

        }
        [HttpPost]
        public ActionResult CrearVenta()
        {
            try
            {
                var detalle = logDetVenta.Instancia.Mostrardetventa();
                double total = 0;

                for (int i = 0; i < detalle.Count(); i++)
                {
                    total += detalle[i].SubTotal;
                }

                entUsuario usu = Session["Usuario"] as entUsuario;

                entUsuario cliente = new entUsuario();
                cliente.IdUsuario = usu.IdUsuario;

                entVenta venta = new entVenta();
                venta.Cliente = cliente;
                venta.Total = total;
                int idventa = logVenta.Instancia.CrearVenta(venta);
                venta.IdVenta = idventa;
                entDetVenta det = new entDetVenta();

                for (int i = 0; i < detalle.Count; i++)
                {
                    det = detalle[i];
                    det.Venta = venta;
                    logDetVenta.Instancia.CrearDetVenta(det);
                }
                detalle.Clear();
                return RedirectToAction("Listar");

            }
            catch
            {
                return RedirectToAction("Listar");

            }
        }
    }
}
