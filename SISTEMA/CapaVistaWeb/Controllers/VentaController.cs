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
    [Authorize]// No puede si es que no esta autorizado
    public class VentaController : Controller
    {
        [PermisosRol(entRol.Cliente)]
        public ActionResult Listar()
        {
            entCliente usu = Session["Usuario"] as entCliente;
            List<entVenta> lista = logVenta.Instancia.ListarVenta(usu.IdCliente);
            ViewBag.lista = lista;
            return View(lista);
        }
        [PermisosRol(entRol.Cliente)]
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

                entCliente usu = Session["Usuario"] as entCliente;

                entCliente cliente = new entCliente();
                cliente.IdCliente = usu.IdCliente;

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
        [PermisosRol(entRol.Cliente)]
        [HttpPost]
        public ActionResult LlenarDetalle(int pCantidad, FormCollection frm)
        {
            try
            {
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
        [PermisosRol(entRol.Cliente)]
        public ActionResult Detalle()
        {
            List<entDetVenta> detalle = logDetVenta.Instancia.Mostrardetventa();
            List<entProducto> listaproducto = logProducto.Instancia.ListarProducto();
            var lsproducto = new SelectList(listaproducto, "idProducto", "nombre");
            ViewBag.listaproducto = lsproducto;
            return View(detalle);
        }
        [PermisosRol(entRol.Cliente)]
        public ActionResult EliminarDetalle(int ids)
        {
            try
            {
                logDetVenta.Instancia.EliminarDetalle(ids);
                return RedirectToAction("Detalle");
            }
            catch
            {
                return RedirectToAction("Detalle");
            }

        }
    }
}
