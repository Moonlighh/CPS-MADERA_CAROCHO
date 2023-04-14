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
        [HttpPost]
        public ActionResult VerificarAcceso(string dato, string contra)
        {
            //entUsuario ousuario = logUsuario.Instancia.ObtenerUsuarios().Where(u => u.Correo == dato && u.Pass == Encriptar.GetSHA256(contra)).FirstOrDefault();
            entUsuario objCliente = logUsuario.Instancia.IniciarSesion(dato, contra);
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

            return RedirectToAction("Index"); //Si es que hay otro tipo igual que te recargue la pagina
        }
        [HttpPost]
        public ActionResult SingUp(string cNombre, string cdni, string ctelefono, string cdireccion, string cusername, string ccorreo, string cpassword, string cpassconfirm, FormCollection frmub, FormCollection frm)
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo crear";
                    } 
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListarAdmin", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("ListarAdmin");
        }       
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;// Terminar la sesión
            FormsAuthentication.SignOut();// Que se cierre la autenticación
            return RedirectToAction("Index");
        }
        // Una sesion almacena toda la informacion de un objeto en el lado del servidor
    }
}