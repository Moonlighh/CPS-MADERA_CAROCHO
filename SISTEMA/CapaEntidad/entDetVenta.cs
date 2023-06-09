using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetVenta
    {
        private int idDetVenta;
        private entProducto producto;
        private entVenta venta;
        private int cantidad;
        private Double preUnitario;
        private decimal subTotal = 0M;

        #region Get and Set
        public int IdDetVenta
        {
            get { return idDetVenta; }
            set { idDetVenta = value; }
        }
        public entProducto Producto
        {
            get { return producto; }
            set { producto = value; }
        }
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }
        public Double PreUnitario
        {
            get { return preUnitario; }
            set { preUnitario = value; }
        }
        public decimal Subtotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }
        public entVenta Venta
        {
            get => venta;
            set => venta = value;
        }
        #endregion
    }
}
