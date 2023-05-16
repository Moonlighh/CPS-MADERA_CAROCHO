using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public interface ILogCompra
    {
        bool CrearCompra(entUsuario user, List<entCarrito> listaProductos);

        List<entCompra> ListarCompras();

        List<entCompra> BuscarCompra(string busqueda);
    }
}
