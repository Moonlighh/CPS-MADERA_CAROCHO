using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;//FormsAutenticathion
using MadereraCarocho.Permisos;//Para los permisos
using System.Collections;

namespace MadereraCarocho.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<entUbigeo> listUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listUbigeo, "idUbigeo", "distrito");
            ViewBag.listUbigeo = lsUbigeo;
            return View(listUbigeo);
        }
        [PermisosRol(entRol.Administrador)]
        [Authorize]// No puede si es que no esta autorizado //Almacena la info en la memoria del navegador
        public ActionResult Admin()
        {
            return View();
        }
        [PermisosRol(entRol.Cliente)]
        [Authorize]// No puede si es que no esta autorizado
        public ActionResult Cliente()
        {
            if (Session["Usuario"] != null)
            {
                entUsuario cliente = Session["Usuario"] as entUsuario;
                ViewBag.Usuario = cliente.RazonSocial;
                return View();
            }
            return RedirectToAction("Index");
        }
        public ActionResult SinPermisos()
        {
            ViewBag.Message = "Usted no tiene permisos para acceder a esta pagina";
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerificarAcceso(string user, string pass)
        {
            try
            {
                if (!(String.IsNullOrEmpty(user) || String.IsNullOrEmpty(pass)))
                {
                    //entUsuario ousuario = logUsuario.Instancia.ObtenerUsuarios().Where(u => u.Correo == dato && u.Pass == Encriptar.GetSHA256(contra)).FirstOrDefault();
                    entUsuario objCliente = logUsuario.Instancia.IniciarSesion(user, pass);
                    if (objCliente != null)
                    {
                        FormsAuthentication.SetAuthCookie(objCliente.Correo, false); //Almacenar autenticacion dentro de una cokkie (segundo parametro es que el obj no sera persistente)
                        Session["Usuario"] = objCliente;// Una sesión puede almacenar cualquier tipo de dato
                        if (objCliente.Rol == entRol.Administrador)
                        {
                            return RedirectToAction("Admin");
                        }
                        else
                        {
                            if (objCliente.Rol == entRol.Cliente)
                            {
                                return RedirectToAction("Cliente");
                            }
                        }
                    }
                    else
                    {
                        TempData["Mensaje"] = "Usuario o contraseña incorrectos";
                        return RedirectToAction("Login");
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Login"); //Si es que hay otro tipo igual que te recargue la pagina
        }
        [HttpPost]
        public ActionResult SingUp(string cNombre, string cdni, string ctelefono, string cdireccion, string cusername, string ccorreo, string cpassword, string cpassconfirm, FormCollection frmub, FormCollection frm)
        {
            try
            {
                if (cpassword == cpassconfirm)
                {
                    entRoll rol = new entRoll
                    {
                        IdRoll = 2
                    };
                    entUbigeo u = new entUbigeo { 
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
                        Roll = rol,
                        Ubigeo = u
                    };
                    List<string> errores = new List<string>();
                    bool creado = logUsuario.Instancia.CrearCliente(c, out errores);
                    if (creado == true && errores.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in errores)
                        {
                            TempData["Error"] += error;
                        }
                        return RedirectToAction("Error");
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
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;// Terminar la sesión
            FormsAuthentication.SignOut();// Que se cierre la autenticación
            return RedirectToAction("Index");
        }
        // Una sesion almacena toda la informacion de un objeto en el lado del servidor
        public ActionResult Error()
        {
            string mensaje = TempData["Error"] as string;
            TempData["Error"] = null; // Limpiar el mensaje de error de la TempData
            ViewBag.Error = mensaje;
            return View();
        }
    }
}