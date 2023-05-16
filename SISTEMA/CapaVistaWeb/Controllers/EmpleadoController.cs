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

        [HttpPost]
        public ActionResult CrearEmpleado(string cNombreE, string cDniE, string cTelefonoE, string cDireccionE, double cSalarioE, string cDescripcionE, FormCollection frm)
        {
            try
            {
                entEmpleado e = new entEmpleado
                {
                    Nombres = cNombreE,
                    Dni = cDniE,
                    Telefono = cTelefonoE,
                    Direccion = cDireccionE,
                    Salario = cSalarioE,
                    Descripcion = cDescripcionE,
                    Tipo = new entTipoEmpleado()
                };
                e.Tipo.IdTipo_Empleado = Convert.ToInt32(frm["cTipo"]);
                e.Tipo.Nombre = frm["cTipoE"];
                e.Ubigeo = new entUbigeo
                {
                    IdUbigeo = frm["cDistrito"],
                    Distrito = frm["cDistritoE"]
                };
                bool inserta = logEmpleado.Instancia.CrearEmpleado(e);
                if (!inserta)
                {
                    TempData["Error"] = "Empleado no se pudo crear";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Se produjo el siguiente error: " + e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Listar");
        }

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

        [HttpGet]
        public ActionResult EditarEmpleado(int idemp)
        {
            entEmpleado emp = new entEmpleado();

            try
            {
                emp = logEmpleado.Instancia.BuscarIdEmpleado(idemp);

                List<entTipoEmpleado> listaTipoEmpleado = logTipoEmpleado.Instancia.ListarTipoEmpleado();
                var lsTipoEmpleado = new SelectList(listaTipoEmpleado, "idTipo_Empleado", "nombre");
                List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
                var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");
                ViewBag.listaTipo = lsTipoEmpleado;
                ViewBag.listaUbigeo = lsUbigeo;
            }
            catch (Exception e)
            {
                TempData["Error"] = "Se produjo el siguiente error: " + e.Message;
                return RedirectToAction("Error", "Home");
            }
            return View(emp);
        }
        
        [HttpPost]
        public ActionResult EditarEmpleado(entEmpleado e, FormCollection frm)
        {
            try
            {
                e.Tipo = new entTipoEmpleado
                {
                    IdTipo_Empleado = Convert.ToInt32(frm["cTipoE"])
                };
                e.Ubigeo = new entUbigeo
                {
                    IdUbigeo = frm["cDistrito"],
                    Distrito = frm["cDistritoE"]
                };
                bool edita = logEmpleado.Instancia.ActualizarEmpleado(e);
                if (!edita)
                {
                    TempData["Error"] = "Empleado no se pudo editar";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Empleado no se pudo editar: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Listar");
        }

        [HttpGet]
        public ActionResult DeshabilitarEmpleado(int idE)
        {
            try
            {
                bool elimina = logEmpleado.Instancia.DeshabilitarEmpleado(idE);
                if (!elimina)
                {
                    TempData["Error"] = "Empleado no se pudo deshabilitar";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Empleado no se pudo deshabilitar: " + e.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Listar");
        }
    }
}