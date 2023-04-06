using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MadereraCarocho.Controllers
{
    [PermisosRol(entRol.Administrador)]// No puede acceder si es que no es administrador
    [Authorize]// No puede si es que no esta autorizado
    public class ClienteController : Controller
    {
        private string mensaje;
        // GET: Cliente
        public ActionResult Listar(string dato)//listar y buscar en el mismo
        {
            List<entCliente> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logCliente.Instancia.BuscarCliente(dato); 
            }
            else
            {
                lista = logCliente.Instancia.ListarCliente();
            }
            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");

            ViewBag.lista = lista;
            ViewBag.listaUbigeo = lsUbigeo;
            ViewBag.Mensaje = mensaje;
            return View(lista);
        }

        [HttpGet]
        public ActionResult ListarAdmin(string dato)
        {
            List<entCliente> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logCliente.Instancia.BuscarUsuarioAdmin(dato);
            }
            else
            {
                lista = logCliente.Instancia.ListarUsuarioAdmin();
            }
            List<entRoll> listaRol = logRoll.Instancia.ListarRol();
            var lsRol = new SelectList(listaRol, "idRoll", "descripcion");

            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");

            ViewBag.lista = lista;
            ViewBag.listaUbigeo = lsUbigeo;
            ViewBag.listaRoll = lsRol;
            return View(lista);
        }

        /*[HttpGet]
        public ActionResult EliminarUsuarioAdmin(int idu)
        {
            try
            {
                bool elimina = logUsuario.Instancia.EliminarUsuaro(idu);
                if (elimina)
                {
                    return RedirectToAction("ListarAdmin");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListarAdmin", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("ListarAdmin");
        }*/


        [HttpGet]
        public ActionResult EliminarCliente(int idP)
        {
            try
            {
                bool elimina = logCliente.Instancia.EliminarCliente(idP);
                if (elimina)
                {
                    mensaje = "Cliente eliminado correctamente";
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