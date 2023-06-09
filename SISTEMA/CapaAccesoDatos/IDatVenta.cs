using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaAccesoDatos
{
    public interface IDatVenta
    {
        bool CrearVenta(entVenta comp, out int idVenta);
        List<entVenta> ListarVenta(int idVenta);
        List<entVenta> BuscarVenta(string busqueda);
    }
}
