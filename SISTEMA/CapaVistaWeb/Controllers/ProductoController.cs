using CapaEntidad;
using CapaLogica;
using MadereraCarocho.Permisos;
using MadereraCarocho.ViewModels;
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
    [PermisosRol(entRol.Administrador)]
    public class ProductoController : Controller
    {
        #region Producto
        [HttpPost]
        public ActionResult CrearProducto(string cNombreP, string cLongitudP, string cDiametro, string cPreVentaP, FormCollection frm)
        {
            try
            {
                entProducto p = new entProducto
                {
                    Nombre = cNombreP,
                    Longitud = double.Parse(cLongitudP),
                    Diametro = double.Parse(cDiametro),
                    PrecioVenta = double.Parse(cPreVentaP),
                    Stock = 0,
                    Tipo = new entTipoProducto()
                };
                p.Tipo.IdTipo_producto = Convert.ToInt32(frm["cTipo"]);

                bool inserta = logProducto.Instancia.CrearProducto(p);
                if (!inserta)
                {
                    TempData["Error"] = "El producto " + cNombreP + "no pudo ser agregado";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("ListarProductos");
        }

        // Esta funcion se encarga de listar todos los productos en donde el stock se acerca a 0
        public ActionResult ListarProductos(string dato, string orden)
        {
            try
            {
                var lista = logProducto.Instancia.ListarProductos(dato, orden);
                List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
                var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");

                ViewBag.lista = lista;
                ViewBag.listaTipo = lsTipoProducto;
                return View(lista);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult EditarProducto(int idProd)
        {
            entProducto prod = new entProducto();
            try
            {
                prod = logProducto.Instancia.BuscarProductoId(idProd);

                List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
                var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");
                ViewBag.listaTipo = lsTipoProducto;
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult EditarProducto(entProducto p, FormCollection frm)
        {
            try
            {
                p.Tipo = new entTipoProducto();
                p.Tipo.IdTipo_producto = Convert.ToInt32(frm["cTipoU"]);


                bool edita = logProducto.Instancia.ActualizarProducto(p);
                if(!edita)
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
            return RedirectToAction("ListarProductos");
        }

        public ActionResult EliminarProducto(int idProd)
        {
            try
            {
                if (logProducto.Instancia.EliminarProducto(idProd))
                    return RedirectToAction("ListarProductos");
                else
                {
                    TempData["Error"] = "Error al eliminar el producto";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Carrito Compra
        public ActionResult AgregarCarrito(int idProveedorProducto)
        {
            MensajeViewModel mensaje = new MensajeViewModel();
            try
            {
                entUsuario admin = Session["Usuario"] as entUsuario;
                bool agregado = logCarrito.Instancia.AgregarProductoCarrito(admin, idProveedorProducto, 1);
                if (agregado)
                {
                    mensaje.Tipo = "exito";
                    mensaje.Titulo = "Éxito";
                    mensaje.Mensaje = "El producto fue agregado con éxito.";
                }
                else
                {
                    mensaje.Tipo = "error";
                    mensaje.Titulo = "Error";
                    mensaje.Mensaje = "No se pudo agregar el producto.";
                }
            }
            catch (Exception e)
            {
                mensaje.Tipo = "error";
                mensaje.Titulo = "Error";
                mensaje.Mensaje = e.Message;
                return RedirectToAction("Error", "Home", mensaje);
            }
            return RedirectToAction("ListarProductosDisponibles");
        }

        // Esta funcion se encarga de listar todos los productos disponibles para ser agregados al carrito de compras
        public ActionResult ListarProductosDisponibles(string dato, string orden)
        {
            List<entProveedorProducto> lista = new List<entProveedorProducto>();
            try
            {
                lista = logProveedorProducto.Instancia.ListarProductoAdmin(dato, orden);
                List<entTipoProducto> listaTipoProducto = logTipoProducto.Instancia.ListarTipoProducto();
                var lsTipoProducto = new SelectList(listaTipoProducto, "idTipo_producto", "tipo");

                ViewBag.lista = lista;
                ViewBag.listaTipo = lsTipoProducto;
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
            return View(lista);
        }
        #endregion
    }
}