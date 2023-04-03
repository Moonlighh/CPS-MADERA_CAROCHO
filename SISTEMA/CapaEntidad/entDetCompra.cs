using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetCompra
    {
        private int idDetCompra;
        private entProducto producto;
        private entCompra compra;
        private int cantidad;
        private Double preUnitario;
        private Double subTotal;

        #region Get and Set
        public int IdDetCompra
        {
            get { return idDetCompra; }
            set { idDetCompra = value; }
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
        public Double Subtotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }

        public entCompra Compra { get => compra; set => compra = value; }
        #endregion Get and Set
    }
}
