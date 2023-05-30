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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ruta específica para la página de inicio
            routes.MapRoute(
                name: "Inicio",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            // Ruta específica para la página de inicio
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Home", action = "Login" }
            );

            // Ruta específica para compras realizadas
            routes.MapRoute(
                name: "Compras",
                url: "compras-realizadas",
                defaults: new { controller = "Compra", action = "ComprasRealizadas" }
            );


            // Ruta para listar productos disponibles
            routes.MapRoute(
                name: "ListarProductosDisponibles",
                url: "Producto/productos-disponibles",
                defaults: new { controller = "Producto", action = "ListarProductosDisponibles", id = UrlParameter.Optional }
            );

            // Ruta para mostrar productos
            routes.MapRoute(
                name: "ListarProductosAdmin",
                url: "Producto/Mostrar-Productos",
                defaults: new { controller = "Producto", action = "ListarProductos", id = UrlParameter.Optional }
            );

            // Ruta para listar clientes
            routes.MapRoute(
                name: "ListarClientes",
                url: "Usuario/Clientes",
                defaults: new { controller = "Usuario", action = "ListarClientes", id = UrlParameter.Optional }
            );

            // Ruta para listar administradores
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
