using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCompra
    {
        private static readonly logCompra _instancia = new logCompra();

        public static logCompra Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public int CrearCompra(entCompra comp)
        {
            return datCompra.Instancia.CrearCompra(comp);
        }
        public List<entCompra> ListarCompra()
        {
            return datCompra.Instancia.ListarCompra();
        }
        public bool EliminarCompra(int comp)
        {
            return datCompra.Instancia.EliminarCompra(comp);
        }
        #endregion

        #region Otros
        public int DevolverID(string tipo)
        {
            return datCompra.Instancia.GenerarID(tipo);
        }
        public List<entCompra> BuscarCompra(string busqueda)
        {
            return datCompra.Instancia.BuscarCompra(busqueda);
        }
        #endregion
    }
}
