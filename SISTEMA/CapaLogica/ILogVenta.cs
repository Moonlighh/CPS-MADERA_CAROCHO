using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public interface ILogVenta
    {
        bool CrearVenta(entUsuario user, List<entCarrito> listaProductos);

        List<entVenta> ListarVentas(int idUsuario);

        List<entVenta> BuscarVenta(string busqueda);
    }
}
