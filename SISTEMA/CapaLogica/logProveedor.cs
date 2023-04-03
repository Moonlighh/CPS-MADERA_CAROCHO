using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logProveedor
    {
        private static readonly logProveedor _instancia = new logProveedor();

        public static logProveedor Instancia
        {
            get { return _instancia;}
        }

        #region CRUD
        public bool CrearProveedor(entProveedor pro)
        {
            return datProveedor.Instancia.CrearProveedor(pro);
        }
        public List<entProveedor> ListarProveedor()
        {
            return datProveedor.Instancia.ListarProveedor();
        }
        public bool ActualizarProveedor(entProveedor pro)
        {
            return datProveedor.Instancia.ActualizarProveedor(pro);
        }
        public bool EliminarProveedor(int id)
        {
            return datProveedor.Instancia.EliminarProveedor(id);
        }
        #endregion CRUD

        public List<entProveedor> BuscarProveedor(string busqueda)
        {
            return datProveedor.Instancia.BuscarProveedor(busqueda);
        }
        public entProveedor BuscarIdProveedor(int idProveedor)
        {
            return datProveedor.Instancia.BuscarIdProveedor(idProveedor);
        }
    }
}
