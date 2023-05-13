using CapaEntidad;
using CapaLogica;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;//FormsAutenticathion
using MadereraCarocho.Permisos;//Para los permisos
using MadereraCarocho.Recursos;//Para los permisos
using CapaAccesoDatos;
using System.Threading.Tasks;

namespace MadereraCarocho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogUsuario _logUsuario = new logUsuario(new datUsuario());
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
                    entUsuario objCliente = _logUsuario.IniciarSesion(user, pass);
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
                TempData["Mensaje"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Error");
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
                    bool creado = _logUsuario.CrearCliente(c, out errores);
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
        [HttpPost]
        public ActionResult CrearSesionUsuario(string cusername, string ccorreo, string cpassword, string cpassconfirm)
        {
            try
            {
                if (cpassword == cpassconfirm)
                {
                    entRoll rol = new entRoll
                    {
                        IdRoll = 2
                    };
                    entUsuario u = new entUsuario(cusername, ccorreo, cpassword, rol);
                    List<string> errores = new List<string>();
                    bool creado = _logUsuario.CrearSesionUsuario(u, out errores);
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
                    TempData["Error"] = "Las contraseñas no coinciden";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ContactForm(string nombre, string email, string asunto, string mensaje)
        {
            List<string> errores = new List<string>();
            try
            {
                // Instanciar la clase Service para verificar si el correo es válido
                Service s = new Service();

                var isValid = true/*await s.IsValidEmail(email, 100)*/;
                // Verificar si el correo es válido
                if (isValid)
                {
                    // Si el correo es válido, crear un objeto de tipo entContactForm con los datos recibidos y la dirección IP del usuario
                    entContactForm frm = new entContactForm(nombre, email, asunto, mensaje, "192.168.10.1");

                    // Crear el formulario en la base de datos
                    bool creado = logContactForm.Instance.CrearFormulario(frm, out errores);
                    if (!creado)
                    {
                        // Si hay errores al crear el formulario, almacenarlos en TempData y redirigir a la página de inicio
                        TempData["MensajeError"] = errores;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Si se creó el formulario exitosamente, mostrar un mensaje de éxito y redirigir a la página de inicio
                        TempData["SuccessMessage"] = "Tu mensaje fue enviado. Gracias!";
                        return Redirect(Url.Action("Index", "Home") + "#section-form");
                    }
                }
                else
                {
                    // Si el correo no es válido, mostrar un mensaje de error y redirigir a la página de inicio
                    errores.Add("No se pudo procesar la solicitud");
                    TempData["MensajeError"] = errores;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {              
                // Si se produce una excepción, mostrar un mensaje de error y redirigir a la página de error
                TempData["Error"] = "Se produjo el siguiente error al procesar la solicitud: " + e.Message;
                return RedirectToAction("Error");
            }
        }

        // Una sesion almacena toda la informacion de un objeto en el lado del servidor
        public ActionResult Error()
        {
            string mensaje = TempData["Error"] as string;
            TempData["Error"] = null; // Limpiar el mensaje de error de la TempData
            ViewBag.Error = mensaje;
            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;// Terminar la sesión
            FormsAuthentication.SignOut();// Que se cierre la autenticación
            return RedirectToAction("Index");
        }
    }
}