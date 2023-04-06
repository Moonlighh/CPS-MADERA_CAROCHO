using CapaEntidad;
using CapaLogica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MadereraCarocho.Controllers
{
    public class ProveedorProductoController : Controller
    {
        // GET: ProveedorProducto
        public ActionResult Listar()
        {
            List<entProveedorProducto> lista;
            lista = logProveedorProducto.Instancia.ListarProveedorProducto();

            List<entUbigeo> listaUbigeo = logUbigeo.Instancia.ListarDistrito();
            var lsUbigeo = new SelectList(listaUbigeo, "idUbigeo", "distrito");

            ViewBag.lista = lista;
            ViewBag.listaUbigeo = lsUbigeo;
            return View(lista);
        }
    }
}