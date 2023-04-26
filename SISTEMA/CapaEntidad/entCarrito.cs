using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCarrito
    {
        private int idCarrito;
        private entUsuario cliente;
        private entProducto producto;
        private int cantidad;
        private double subtotal;

        public int IdCarrito { get => idCarrito; set => idCarrito = value; }
        public entUsuario Cliente { get => cliente; set => cliente = value; }
        public entProducto Producto { get => producto; set => producto = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public double Subtotal { get => subtotal; set => subtotal = value; }
    }
}
