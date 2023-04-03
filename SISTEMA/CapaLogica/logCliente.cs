using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCliente
    {
        private static readonly logCliente _instancia = new logCliente();
        public static logCliente Instancia
        {
            get { return _instancia; }
        }
        #region CRUD
        public int CrearCliente(entCliente c)
        {
            return datCliente.Instacia.CrearCliente(c);
        }
        public List<entCliente> ListarCliente()
        {
            return datCliente.Instacia.ListarCliente();
        }
        public bool ActualizarCliente(entCliente c)
        {
            return datCliente.Instacia.ActualizarCliente(c);
        }
        public bool EliminarCliente(int id)
        {
            return datCliente.Instacia.EliminarCliente(id);
        }
        #endregion CRUD

        public List<entCliente> BuscarCliente(string dato)
        {
            return datCliente.Instacia.BuscarCliente(dato);
        }
        public entCliente BuscarIdCliente(int idCliente)
        {
            return datCliente.Instacia.BuscarIdCliente(idCliente);
        }
    }
}
