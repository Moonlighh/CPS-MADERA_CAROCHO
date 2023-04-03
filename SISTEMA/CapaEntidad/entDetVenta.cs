using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetVenta
    {
        private int idDetventa;
        private entVenta venta;
        private entProducto producto;
        private int cantidad;
	    private double subTotal;


        #region Get and Set
        public int IdDetventa { get => idDetventa; set => idDetventa = value; }
        public entVenta Venta
        {
            get { return venta; }
            set { venta = value; }
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
      
        public Double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }

        
        #endregion Get and Set

    }
}
