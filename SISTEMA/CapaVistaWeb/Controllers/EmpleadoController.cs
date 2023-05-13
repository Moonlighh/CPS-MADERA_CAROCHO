using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MadereraCarocho.Controllers
{
    [PermisosRol(entRol.Administrador)]
    [Authorize]// No puede si es que no esta autorizado
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Listar(string busqueda, string orden)
        {
            try
            {
                var lista = logEmpleado.Instancia.ListarEmpleado(busqueda, orden);
                List<entTipoEmpleado> listaTipoEmpleado = logTipoEmpleado.Instancia.ListarTipoEmpleado();
                var lsTipoEmpleado = new SelectList(listaTipoEmpleado, "idTipo_Empleado", "nombre");
                List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
                var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");
                ViewBag.lista = lista;
                ViewBag.listaTipo = lsTipoEmpleado;
                ViewBag.listaUbigeo = lsUbigeo;
                return View(lista);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult CrearEmpleado(string cNombreE, string cDniE, string cTelefonoE, string cDireccionE, 
            /*DateTime cF_inicioE, DateTime cf_finE,*/ double cSalarioE, string cDescripcionE, 
            /*bool cEstEmpleadoE,*/ FormCollection frm)
        {
            try
            {
                entEmpleado e = new entEmpleado();
                e.Nombres = cNombreE;
                e.Dni = cDniE;
                e.Telefono = cTelefonoE;
                e.Direccion = cDireccionE;
                //e.F_inicio = Convert.ToDateTime(cF_inicioE);
                //e.F_fin = Convert.ToDateTime(cf_finE);
                e.Salario = cSalarioE;
                e.Descripcion = cDescripcionE;
                //e.EstEmpleado = cEstEmpleadoE;
                e.Tipo = new entTipoEmpleado();
                e.Tipo.IdTipo_Empleado = Convert.ToInt32(frm["cTipo"]);
                e.Tipo.Nombre = frm["cTipoE"];
                e.Ubigeo = new entUbigeo();
                e.Ubigeo.IdUbigeo = frm["cDistrito"];
                e.Ubigeo.Distrito = frm["cDistritoE"];
                bool inserta = logEmpleado.Instancia.CrearEmpleado(e);
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

        [HttpGet]
        public ActionResult EditarEmpleado(int idemp)
        {
            entEmpleado emp = new entEmpleado();
            emp = logEmpleado.Instancia.BuscarIdEmpleado(idemp);

            List<entTipoEmpleado> listaTipoEmpleado = logTipoEmpleado.Instancia.ListarTipoEmpleado();
            var lsTipoEmpleado = new SelectList(listaTipoEmpleado, "idTipo_Empleado", "nombre");
            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");
            ViewBag.listaTipo = lsTipoEmpleado;
            ViewBag.listaUbigeo = lsUbigeo;
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditarEmpleado(entEmpleado e, FormCollection frm)
        {
            e.Tipo = new entTipoEmpleado();
            e.Tipo.IdTipo_Empleado = Convert.ToInt32(frm["cTipoE"]);
            e.Ubigeo = new entUbigeo();
            e.Ubigeo.IdUbigeo = frm["cDistrito"];
            e.Ubigeo.Distrito = frm["cDistritoE"];
            try
            {
                Boolean edita = logEmpleado.Instancia.ActualizarEmpleado(e);
                if (edita)
                {
                    return RedirectToAction("Listar");
                }
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExceptio = ex.Message });
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public ActionResult DeshabilitarEmpleado(int idE)
        {
            try
            {
                bool elimina = logEmpleado.Instancia.DeshabilitarEmpleado(idE);
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