using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entProveedorProducto
    {
        private int idProveedorProducto;
        private entProveedor proveedor;
        private entProducto producto;
        private double precioCompra;
        private double precioVenta;

        #region Get and Set
        public int IdProveedorProducto { get => idProveedorProducto; set => idProveedorProducto = value; }
        public entProveedor Proveedor { get => proveedor; set => proveedor = value; }
        public entProducto Producto { get => producto; set => producto = value; }
        public double PrecioCompra { get => precioCompra; set => precioCompra = value; }
        public double PrecioVenta { get => precioVenta; set => precioVenta = value; }
        #endregion
    }
}
