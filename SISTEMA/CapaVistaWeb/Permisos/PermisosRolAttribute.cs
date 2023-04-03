using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;//Para ActionFilterAttribute

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
                entCliente cliente = HttpContext.Current.Session["Usuario"] as entCliente;// Convertimos la sesion que contiene la info del usuario se convierta al tipo usuario
                if (cliente.Rol != idRol)
                {
                    filterContext.Result = new RedirectResult("~/Home/SinPermisos");
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