using CapaAccesoDatos;
using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MadereraCarocho.Controllers
{
    [PermisosRol(entRol.Administrador)]// No puede acceder si es que no es administrador
    [Authorize]// No puede si es que no esta autorizado
    public class UsuarioController : Controller
    {
        private readonly ILogUsuario _logUsuario;
        public UsuarioController() {
            _logUsuario = new logUsuario(new datUsuario());
        }
        public ActionResult ListarUsuarios(string dato)//listar y buscar en el mismo
        {
            List<entUsuario> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = _logUsuario.BuscarUsuario(dato);
            }
            else
            {
                lista = _logUsuario.ListarUsuarios();
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
        public ActionResult ListarClientes(string dato, string orden)//listar y buscar en el mismo
        {
            var lista = _logUsuario.ListarClientes(dato, orden);
            List<entRoll> listaRol = logRoll.Instancia.ListarRol();
            var lsRol = new SelectList(listaRol, "idRoll", "descripcion");
            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");

            ViewBag.lista = lista;
            ViewBag.listaUbigeo = lsUbigeo;
            ViewBag.listaRoll = lsRol;
            return View(lista);
        }

        public ActionResult ListarAdministradores(string dato, string orden)
        {
            var lista = _logUsuario.ListarAdministradores(dato, orden);
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
                    entRoll rol = new entRoll
                    {
                        IdRoll = 2
                    };
                    entUbigeo u = new entUbigeo
                    {
                        IdUbigeo = frmub["cUbi"].ToString()
                    };
                    entUsuario c = new entUsuario
                    {
                        RazonSocial = cNombre,
                        Dni = cdni,
                        Telefono = ctelefono,
                        Direccion = cdireccion,
                        UserName = cusername,
                        Correo = ccorreo,
                        Pass = cpassword,
                        Roll = rol
                    };
                    List<string> errores = new List<string>();
                    bool creado = _logUsuario.CrearCliente(c, out errores);
                    if (creado == true && errores.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in errores)
                        {
                            TempData["Error"] += error + "\n";
                        }
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Informacion = "Las contras no coinciden" });

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult HabilitarUsuario(int idU)
        {
            try
            {
                bool habilitar = _logUsuario.HabilitarUsuario(idU);
                if (habilitar)
                {
                    return RedirectToAction("ListarUsuarios");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("ListarUsuarios");
        }
        [HttpGet]
        public ActionResult DeshabilitarCliente(int idC)
        {
            try
            {
                bool elimina = _logUsuario.DeshabilitarUsuario(idC);
                if (elimina)
                {
                    return RedirectToAction("ListarClientes");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public ActionResult DeshabilitarAdmin(int idA)
        {
            try
            {
                bool elimina = _logUsuario.DeshabilitarUsuario(idA);
                if (elimina)
                {
                    return RedirectToAction("ListarAdministradores");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Error", "Home");
        }
    }
}