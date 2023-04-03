using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MadereraCarocho.Controllers
{
    
    [Authorize]// No puede si es que no esta autorizado
    public class ProductoController : Controller
    {
        // GET: Producto
        [PermisosRol(entRol.Administrador)]
        public ActionResult Listar(string dato)//listar y buscar
        {
            List<entProducto> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logProducto.Instancia.BuscarProducto(dato);
            }
            else
            {
                lista = logProducto.Instancia.ListarProducto();
            }
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "nombre");

            ViewBag.lista = lista;
            ViewBag.listaTipo = lsTipoProducto;
            return View(lista);
        }
        
        [PermisosRol(entRol.Cliente)]
        //pra la vista de clientes
        //[HttpGet]
        public ActionResult Productos(string data)
        {
            List<entProducto> lista;
            if (!String.IsNullOrEmpty(data))
            {
                lista = logProducto.Instancia.BuscarProducto(data);
            }
            else
            {
                lista = logProducto.Instancia.ListarProducto();
            }
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "nombre");

            ViewBag.lista = lista;
            ViewBag.listaTipo = lsTipoProducto;
            return View(lista);
        }


        [PermisosRol(entRol.Administrador)]
        [HttpPost]
        public ActionResult CrearProducto(string cNombreP, string cLongitudP, string cPreCompraP, string cPreVentaP, FormCollection frm)
        {
            try
            {
                entProducto p = new entProducto();
                p.Nombre = cNombreP;
                p.Longitud = Double.Parse(cLongitudP);
                p.PrecioCompra = Double.Parse(cPreCompraP);
                p.PrecioVenta = Double.Parse(cPreVentaP);
                p.Tipo = new entTipoProducto();
                p.Tipo.IdTipo_producto = Convert.ToInt32(frm["cTipo"]);
                bool inserta = logProducto.Instancia.CrearProducto(p);
                if (inserta)
                {
                    return RedirectToAction("Listar");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listar", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Listar");
        }
       
        
        [PermisosRol(entRol.Administrador)]
        [HttpGet]
        public ActionResult EditarProducto(int idprod)
        {
            entProducto prod = new entProducto();
            prod = logProducto.Instancia.BuscarProductoId(idprod);

            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "nombre");
            ViewBag.listaTipo = lsTipoProducto;
            return View(prod);
        }
        [PermisosRol(entRol.Administrador)]


        [HttpPost]
        public ActionResult EditarProducto(entProducto p, FormCollection frm)
        {
            p.Tipo = new entTipoProducto();
            p.Tipo.IdTipo_producto = Convert.ToInt32( frm["cTipoU"]);
            try
            {
                Boolean edita = logProducto.Instancia.ActualizarProducto(p);
                if (edita)
                {
                    return RedirectToAction("Listar");
                }
                else
                {
                    return View(p);
                }
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction("Listar", new { mesjExceptio = ex.Message });
            }
        }
    

        [HttpGet]
        public ActionResult EliminarProducto(int idP)
        {
            try
            {
                bool elimina = logProducto.Instancia.EliminarProducto(idP);
                if (elimina)
                {
                    return RedirectToAction("Listar");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Listar", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Listar");
        }

        [HttpGet]
        public ActionResult Ordenar(int dato)
        {
            List <entProducto> lista= logProducto.Instancia.Ordenar(dato);
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "nombre");

            ViewBag.listaTipo = lsTipoProducto;
            ViewBag.lista = lista;
            return RedirectToAction("Listar");
        }

    }
}