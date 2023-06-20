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
using System.Web;

namespace MadereraCarocho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogUsuario _logUsuario = new logUsuario(new datUsuario());
        
        #region Vistas
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
            if (Session["Usuario"] != null)
            {
                entUsuario cliente = Session["Usuario"] as entUsuario;
                ViewBag.Usuario = cliente.RazonSocial;
                return View();
            }
            return RedirectToAction("Index");
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

        #endregion Vistas

        #region Acceso, Crear cuenta
        [ValidateAntiForgeryToken]
        public ActionResult VerificarAcceso(string user, string pass)
        {
            try
            {
                entUsuario objCliente = _logUsuario.IniciarSesion(user, pass);
                if (objCliente != null)
                {
                    FormsAuthentication.SetAuthCookie(objCliente.Correo, false);
                    Session["Usuario"] = objCliente;
                    if (objCliente.Rol == entRol.Administrador)
                    {
                        return RedirectToAction("Admin");
                    }
                    if (objCliente.Rol == entRol.Cliente)
                    {
                        return RedirectToAction("Cliente");
                    }
                }
                else
                {
                    TempData["Restablecer"] = "¿Olvidaste tu contraseña?";
                    TempData["Mensaje"] = "Usuario o contraseña incorrectos";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error");
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult CrearSesionUsuario(string user, string correo, string pass, string confirmPass)
        {
            try
            {
                if (pass == confirmPass)
                {
                    List<string> errores = new List<string>();
                    bool creado = _logUsuario.CrearSesionUsuario(user, correo, pass, out errores);

                    if (creado)
                    {
                        return RedirectToAction("Index"); // Redireccionar al método "Index" si el usuario se creó correctamente
                    }
                    else
                    {
                        foreach (var error in errores)
                        {
                            TempData["Error"] += error; // Agregar mensajes de error al TempData
                        }

                        return RedirectToAction("Error"); // Redireccionar al método "Error" si ocurrió un error en la creación del usuario
                    }
                }
                else
                {
                    TempData["Error"] = "Las contraseñas no coinciden"; // Establecer un mensaje de error en TempData
                    return RedirectToAction("Error"); // Redireccionar al método "Error" si las contraseñas no coinciden
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message; // Establecer un mensaje de error en TempData
                return RedirectToAction("Error"); // Redireccionar al método "Error" si ocurrió una excepción
            }
        }
        #endregion

        #region Otros
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
                        return Redirect(Url.Action("Index", "Home") + "#section-form");
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
                    return Redirect(Url.Action("Index", "Home") + "#section-form");
                }
            }
            catch (Exception e)
            {
                // Si se produce una excepción, mostrar un mensaje de error y redirigir a la página de error
                TempData["Error"] = "Se produjo el siguiente error al procesar la solicitud: " + e.Message;
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult RestablecerPassword()
        {
            return View();
        }

        public static bool enviado = false;
        private static string correoValidado = string.Empty;       
        [HttpPost]     
        public ActionResult RestablecerPassword(string correo, string newPassword, string codigo)
        {
            string msjCorreoEnviado = $"Se ha enviado un correo electrónico a {correo} con instrucciones para restablecer su contraseña. Por favor, revise su bandeja de entrada y siga los pasos indicados en el correo electrónico para completar el proceso de restablecimiento de contraseña. Si no encuentra el correo electrónico, revise su carpeta de correo no deseado o spam.";
            string msjCodigoNoEnviado = "El correo no se pudo enviar a la direccion proporcionada";
            try
            {
                // Aún no se envio el codigo al correo
                if (!enviado)
                {
                    // Intentar enviar el codigo de restablecimiento
                    enviado = _logUsuario.EnviarCodigoRestablecerPass(correo);

                    // Generar una alerta para cada caso
                    if (enviado)
                    {
                        msjCorreoEnviado.Replace(correo, correoValidado);
                        TempData["TipoAlerta"] = "success";
                        TempData["ContenidoAlerta"] = msjCorreoEnviado;
                    }
                    else
                    {
                        TempData["TipoAlerta"] = "warning";
                        TempData["ContenidoAlerta"] = msjCodigoNoEnviado;
                    }
                }
                else
                {                  
                    // Seguir mostrando el mensaje
                    TempData["TipoAlerta"] = "success";
                    TempData["ContenidoAlerta"] = msjCorreoEnviado;

                    // Verificar que se ingreso el codigo y la nueva contraseña
                    if (!string.IsNullOrWhiteSpace(newPassword) && !string.IsNullOrWhiteSpace(codigo))
                    
                    {
                        // Restablecer los datos con las con las credenciales
                        bool restablecer = _logUsuario.RestablecerPassword(newPassword, codigo);
                        if (restablecer)
                        {
                            return RedirectToAction("Index");
                        }
                        else {
                            TempData["TipoAlerta"] = "danger";
                            TempData["ContenidoAlerta"] = $"No se pudo actualizar la contraseña de la cuenta asociada  a {correoValidado}";
                            return View(); 
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["TipoAlerta"] = "danger";
                TempData["ContenidoAlerta"] = e.Message;
            }
            return View();
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
        #endregion
    }
}