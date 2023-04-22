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
    public class UsuarioController : Controller
    {
        public ActionResult ListarUsuarios(string dato)//listar y buscar en el mismo
        {
            List<entUsuario> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logUsuario.Instancia.BuscarUsuario(dato);
            }
            else
            {
                lista = logUsuario.Instancia.ListarUsuarios();
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
        // GET: Cliente
        public ActionResult ListarCliente(string dato)//listar y buscar en el mismo
        {
            List<entUsuario> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logUsuario.Instancia.BuscarCliente(dato); 
            }
            else
            {
                lista = logUsuario.Instancia.ListarClientes();
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

        [HttpGet]
        public ActionResult ListarAdmin(string dato)
        {
            List<entUsuario> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logUsuario.Instancia.BuscarAdministrador(dato);
            }
            else
            {
                lista = logUsuario.Instancia.ListarAdministradores();
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
        [HttpPost]
        public ActionResult CrearCuenta(string cNombre, string cdni, string ctelefono, string cdireccion, string cusername, string ccorreo, string cpassword, string cpassconfirm, FormCollection frmub, FormCollection frm)
        {
            try
            {
                if (cpassword == cpassconfirm)
                {
                    entUsuario c = new entUsuario();
                    c.RazonSocial = cNombre;
                    c.Dni = cdni;
                    c.Telefono = ctelefono;
                    c.Direccion = cdireccion;
                    c.Ubigeo = new entUbigeo();
                    c.Ubigeo.IdUbigeo = frmub["cUbi"].ToString();
                    c.UserName = cusername;
                    c.Correo = ccorreo;
                    c.Pass = cpassword;
                    entRoll rol = new entRoll();
                    rol.IdRoll = 2;
                    c.Roll = rol;
                    bool creado = logUsuario.Instancia.CrearCliente(c);
                    if (creado)
                    {
                        return RedirectToAction("ListarUsuarios");
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo crear";
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListarUsuarios", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("ListarUsuarios");
        }
        [HttpGet]
        public ActionResult DeshabilitarUsuario(int idu)
        {
            try
            {
                bool elimina = logUsuario.Instancia.DeshabilitarUsuario(idu);
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
        }
    }
}