using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MadereraCarocho.Controllers
{

    [Authorize]// No puede si es que no esta autorizado
    public class ProductoController : Controller
    {
        [PermisosRol(entRol.Administrador)]
        [HttpPost]
        public ActionResult CrearProducto(string cNombreP, string cLongitudP, string cDiametro, string cPreVentaP, FormCollection frm)
        {
            try
            {
                entProducto p = new entProducto();
                p.Nombre = cNombreP;
                p.Longitud = Double.Parse(cLongitudP);
                p.Diametro = Double.Parse(cDiametro);
                p.PrecioVenta = Double.Parse(cPreVentaP);
                p.Stock = 0;
                p.Tipo = new entTipoProducto();
                p.Tipo.IdTipo_producto = Convert.ToInt32(frm["cTipo"]);

                bool inserta = logProducto.Instancia.CrearProducto(p);
                if (inserta)
                {
                    return RedirectToAction("ListarProductos");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ListarProductos", new { mesjExeption = ex.Message });
            }
            return RedirectToAction("ListarProductos");
        }

        [PermisosRol(entRol.Administrador)]
        public ActionResult ListarProductos(string dato)//listar y buscar
        {
            List<entProducto> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logProducto.Instancia.BuscarProducto(dato);
            }
            else
            {
                lista = logProducto.Instancia.ListarProducto();
            }
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");

            ViewBag.lista = lista;
            ViewBag.listaTipo = lsTipoProducto;
            return View(lista);
        }


        [PermisosRol(entRol.Administrador)]
        public ActionResult ListarProductosDisponibles(string dato)//listar y buscar
        {
            List<entProveedorProducto> lista;
            if (!String.IsNullOrEmpty(dato))
            {
                lista = logProveedorProducto.Instancia.BuscarProductoAdmin(dato);
            }
            else
            {
                lista = logProveedorProducto.Instancia.ListarProductoAdmin();
            }
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");

            ViewBag.lista = lista;
            ViewBag.listaTipo = lsTipoProducto;
            return View(lista);
        }


        [PermisosRol(entRol.Administrador)]
        [HttpGet]
        public ActionResult EditarProducto(int idProd)
        {
            entProducto prod = new entProducto();
            prod = logProducto.Instancia.BuscarProductoId(idProd);

            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");
            ViewBag.listaTipo = lsTipoProducto;
            return View(prod);
        }
        [PermisosRol(entRol.Administrador)]


        [HttpPost]
        public ActionResult EditarProducto(entProducto p, FormCollection frm)
        {
            try
            {
                p.Tipo = new entTipoProducto();
                p.Tipo.IdTipo_producto = Convert.ToInt32(frm["cTipoU"]);


                Boolean edita = logProducto.Instancia.ActualizarProducto(p);
                if (edita)
                {
                    return RedirectToAction("ListarProductos");
                }
                else
                {
                    TempData["Error"] = "Asegurate de haber ingresado todos los datos";
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (ApplicationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public ActionResult EliminarProducto(int idProd)
        {
            try
            {
                if (logProducto.Instancia.EliminarProducto(idProd))
                {
                    return RedirectToAction("ListarProductos");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Ordenar(int dato)
        {
            List<entProducto> lista = logProducto.Instancia.Ordenar(dato);
            List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
            var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");

            ViewBag.listaTipo = lsTipoProducto;
            ViewBag.lista = lista;
            return RedirectToAction("ListarProductos");
        }

        [HttpGet]
        public ActionResult AgregarCarrito(int idProveedorProducto)
        {
            try
            {
                entUsuario admin = Session["Usuario"] as entUsuario;

                entCarrito carrito = logCarrito.Instancia.MostrarDetCarrito(admin.IdUsuario).Where(car => car.ProveedorProducto.IdProveedorProducto == idProveedorProducto).FirstOrDefault();

                if (carrito != null)
                {
                    TempData["Error"] = "No puedes agregar el mismo producto dos veces";
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    entProveedorProducto detalle = logProveedorProducto.Instancia.ListarProveedorProducto().Where(d => d.IdProveedorProducto == idProveedorProducto).FirstOrDefault();
                    if (detalle != null)
                    {
                        entCarrito car = new entCarrito
                        {
                            Cliente = admin,
                            ProveedorProducto = detalle,
                            Cantidad = 1,
                            Subtotal = detalle.PrecioCompra
                        };
                        logCarrito.Instancia.AgregarProductoCarrito(car);
                        return RedirectToAction("ListarProductosDisponibles");
                    }
                    else
                    {
                        TempData["Error"] = "Ocurrio un error inesperado. Porfavor intentelo de nuevo o mas tarde";
                        return RedirectToAction("Error", "Home");
                    }
                }

            }
            catch
            {
                TempData["Error"] = "No se pudo agregar el producto";
                return RedirectToAction("Error", "Home");
            }
        }
    }
}