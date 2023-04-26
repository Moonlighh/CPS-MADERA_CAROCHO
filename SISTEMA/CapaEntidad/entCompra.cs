using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCompra
    {
        private int idCompra;
        private DateTime fecha;
        private double total;
        private bool estado;
        private entUsuario usuario;

        #region Get and Set
        public int IdCompra { 
            get { return idCompra; }
            set { idCompra = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        public bool Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        public entUsuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        #endregion
    }
}
