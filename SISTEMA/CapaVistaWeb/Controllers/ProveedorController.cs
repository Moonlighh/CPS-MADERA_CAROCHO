using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace MadereraCarocho.Controllers
{
    [PermisosRol(entRol.Administrador)]
    [Authorize]// No puede si es que no esta autorizado
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        //[HttpGet]
        public ActionResult Listar(string dato, string orden)//listar y buscar 
        {
            var lista = logProveedor.Instancia.ListarProveedor(dato, orden);
            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");
            ViewBag.lista = lista;
            ViewBag.listaUbigeo = lsUbigeo;
            return View(lista);
        }

        [HttpPost]
        public ActionResult CrearProveedor(string uNombre, string uRuc, string uCorreo, string uTelefono, string uDescripcion, FormCollection frm)
        {
            try
            {
                entProveedor p = new entProveedor
                {
                    RazonSocial = uNombre,
                    Dni = uRuc,
                    Correo = uCorreo,
                    Telefono = uTelefono,
                    Descripcion = uDescripcion,
                    EstProveedor = true,
                    Ubigeo = new entUbigeo()
                };
                p.Ubigeo.IdUbigeo =frm["Ubi"].ToString();
                bool inserta = logProveedor.Instancia.CrearProveedor(p);
                if (!inserta)
                {
                    TempData["Error"] = "Fallo al crear el detalle del proveedor";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message; 
                return RedirectToAction("Listar", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Listar");
        }


        [HttpGet]
        public ActionResult EditarProveedor(int idprov)
        {
            entProveedor prov = new entProveedor();
            try
            {
                prov = logProveedor.Instancia.BuscarIdProveedor(idprov);

                List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
                var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");
                ViewBag.listaUbigeo = lsUbigeo;
            }
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return View(prov);
        }
        [HttpPost]
        public ActionResult EditarProveedor(entProveedor p, FormCollection frm)
        {
            p.Ubigeo = new entUbigeo();
            p.Ubigeo.IdUbigeo =frm["Ubi"];
            try
            {
                Boolean edita = logProveedor.Instancia.ActualizarProveedor(p);
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
        public ActionResult DeshabilitarProveedor(int idc)
        {
            try
            {
                bool elimina = logProveedor.Instancia.DeshabilitarProveedor(idc);
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
    }
}