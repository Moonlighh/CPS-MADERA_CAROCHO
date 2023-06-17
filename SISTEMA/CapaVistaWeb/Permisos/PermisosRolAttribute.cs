using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;//Para ActionFilterAttribute
using Newtonsoft.Json.Linq;// Validar correos validos
using System.Net.Http;// Validar correos validos
using System.Threading.Tasks;// Validar correos validos

namespace MadereraCarocho.Permisos
{
    // Resumen:
    //      Valida que al momento que se ejecute una accion valida cierta acción.
    public class PermisosRolAttribute: ActionFilterAttribute //Le decimos que va a heredar de action filter
    {
        private entRol idRol;

        public PermisosRolAttribute(entRol idRol)
        {
            this.idRol = idRol;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Primero validamos que exista una sesión
            if (HttpContext.Current.Session["Usuario"] != null)
            {
                entUsuario usuario = HttpContext.Current.Session["Usuario"] as entUsuario;// Convertimos la sesion que contiene la info del usuario se convierta al tipo usuario
                if (usuario.Rol != idRol)
                {
                    filterContext.Result = new RedirectResult("~/Home/Login");
                    //Con ~ le decimos que se rediriga a la ubicación del proyecto
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            base.OnActionExecuted(filterContext);
        }
    }
}