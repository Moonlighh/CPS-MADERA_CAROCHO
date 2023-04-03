using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCliente
    {
        private int idCliente;
        private string razonSocial;
        private string dni;
        private string telefono;
        private string direccion;
        private entUbigeo ubigeo;


        #region Get and Set
        public int IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }
        public string Dni
        {
            get { return dni; }
            set { dni = value; }

        }
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        public entUbigeo Ubigeo
        {
            get { return ubigeo; }
            set { ubigeo = value; }
        }
        #endregion Get and Set
    }
}
