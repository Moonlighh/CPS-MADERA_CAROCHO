using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoDatos
{
    public interface IDatCompra
    {
        bool CrearCompra(entCompra comp, out int idCompra);
        List<entCompra> ListarCompra();
        bool EliminarCompra(int idcompra);
        List<entCompra> BuscarCompra(string busqueda);
    }
}
