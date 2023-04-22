using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entVenta
    {
        private int idVenta;
        private DateTime fecha;
        private double total;
        private bool estado;
        private entUsuario cliente;

        #region Get and Set
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public Double Total
        {
            get { return total; }
            set { total = value; }
        }
        public bool Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        public entUsuario Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }
        #endregion

    }
}
