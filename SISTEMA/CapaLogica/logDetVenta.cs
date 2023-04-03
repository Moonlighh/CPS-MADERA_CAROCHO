using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaAccesoDatos;
namespace CapaLogica
{
    public class logDetVenta
    {
        private static readonly logDetVenta _instancia = new logDetVenta();

        public static logDetVenta Instancia
        {
            get { return _instancia; }
        }

        public bool CrearDetVenta(entDetVenta det)
        {
            return datDetVenta.Instancia.CrearDetVenta(det);
        }
        public List<entReporteVenta> MostrarReporteVenta(int idVenta)
        {
            return datDetVenta.Instancia.MostrarReporteVenta(idVenta);
        }
        //Carrito compras
        public bool LlenarDetventa(entDetVenta det)
        {
            return datDetVenta.Instancia.Llenardetventa(det);
        }
        public List<entDetVenta> Mostrardetventa()
        {
            return datDetVenta.Instancia.Mostrardetventa();
        }
        public bool EliminarDetalle(int id)
        {
            return datDetVenta.Instancia.Eliminardetalle(id);
        }



    }
}
