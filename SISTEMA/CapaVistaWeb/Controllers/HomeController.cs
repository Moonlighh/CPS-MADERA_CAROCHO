using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using MadereraCarocho.Utilidades;
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
            entUsuario objUsuario = logUsuario.Instancia.IniciarSesion(dato, contra);
            if (objUsuario != null)
            {
                FormsAuthentication.SetAuthCookie(objUsuario.Correo, false); //Almacenar autenticacion dentro de una cokkie (segundo parametro es que el obj no sera persistente)
                Session["Usuario"] = objUsuario;// Una sesión puede almacenar cualquier tipo de dato
                if (objUsuario.Rol == entRol.Administrador)
                {
                    return RedirectToAction("Admin"); 
                }
                else
                {
                    if (objUsuario.Rol == entRol.Cliente)
                    {
                        return RedirectToAction("Cliente");
                    }
                }
            }

            return RedirectToAction("Index"); //Si es que hay otro tipo igual que te recargue la pagina
        }

        [HttpPost]
        public ActionResult CrearCliente(string cNombre, string cdni, string ctelefono, string cdireccion, string cusername, string ccorreo, string cpassword, FormCollection frm)
        {
            try
            {
                entCliente c = new entCliente();
                c.RazonSocial = cNombre;
                c.Dni = cdni;
                c.Telefono = ctelefono;
                c.Direccion = cdireccion;
                c.Ubigeo = new entUbigeo();
                c.Ubigeo.IdUbigeo = frm["cUbi"].ToString();
                
                int idCliente = logCliente.Instancia.CrearCliente(c);
                if (idCliente != -1)
                {
                    entUsuario u = new entUsuario();
                    u.Cliente = c;
                    u.Cliente.IdCliente = idCliente;
                    u.UserName = cusername;
                    u.Correo = ccorreo;
                    u.Pass = Encriptar.GetSHA256(cpassword);
                    entRoll rol = new entRoll();
                    rol.Idrol = 2;//Le mando dos porque es un cliente
                    u.Roll = rol;
                    bool creado = logUsuario.Instancia.CrearUsuario(u);
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
                return RedirectToAction("Index", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("Index");
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