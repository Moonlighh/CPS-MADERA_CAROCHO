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

        private decimal total;

        private bool estado;
        private entUsuario usuario;

        public entVenta() { }

        public entVenta(int idVenta, DateTime fecha, decimal total, bool estado, entUsuario usuario)
        {
            this.idVenta = idVenta;
            this.fecha = fecha;
            this.total = total;
            this.estado = estado;
            this.usuario = usuario;
        }

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
        public decimal Total
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
