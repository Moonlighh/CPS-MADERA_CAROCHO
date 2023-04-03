using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logVenta
    {
        private static readonly logVenta _instancia = new logVenta();

        public static logVenta Instancia
        {
            get { return _instancia; }
        }

        public int CrearVenta(entVenta v)
        {
            return datVenta.Instancia.CrearVenta(v);
        }
        public List<entVenta> ListarVenta(int id)
        {
            return datVenta.Instancia.ListarVenta( id);
        }
        public List<entVenta> ListarVentaPagada(DateTime fecha)
        {
            return datVenta.Instancia.ListarVentaPagada(fecha);
        }
        public List<entVenta> ListarVentaNoPagada(DateTime fecha)
        {
            return datVenta.Instancia.ListarVentaNoPagada(fecha);
        }
        public bool ActualizarVenta(int idVenta, bool estado)
        {
            return datVenta.Instancia.ActualizarVenta(idVenta, estado);
        }
    }
}
