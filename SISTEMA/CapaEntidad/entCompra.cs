using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCompra
    {
        private int idCompra;
        private DateTime fecha;

        [Range(1, 2, ErrorMessage = "El total de la compra debe ser mayor o igual a 1")]
        private decimal total;

        private bool estado;
        private entUsuario usuario;

        public entCompra() { }

        public entCompra(int idCompra, DateTime fecha, decimal total, bool estado, entUsuario usuario)
        {
            this.idCompra = idCompra;
            this.fecha = fecha;
            this.total = total;
            this.estado = estado;
            this.usuario = usuario;
        }
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
