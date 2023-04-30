using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MadereraCarocho
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*
             * ADMINISTRADOR
             */

            //Ruta especifica
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                routes.MapRoute(
                name: "ListarProductosDisponibles",
                url: "Producto/productos-disponibles",
                defaults: new { controller = "Producto", action = "ListarProductosDisponibles", id = UrlParameter.Optional }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                routes.MapRoute(
                name: "ListarProductosAdmin",
                url: "Producto/Mostrar-Productos",
                defaults: new { controller = "Producto", action = "ListarProductos", id = UrlParameter.Optional }
            );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                routes.MapRoute(
                name: "ListarClientes",
                url: "Usuario/Clientes",
                defaults: new { controller = "Usuario", action = "ListarClientes", id = UrlParameter.Optional }
             );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
                routes.MapRoute(
             name: "ListarAdministradores",
                url: "Usuario/Administradores",
            defaults: new { controller = "Usuario", action = "ListarAdministradores", id = UrlParameter.Optional }
             );









            // Ruta predeterminada
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
