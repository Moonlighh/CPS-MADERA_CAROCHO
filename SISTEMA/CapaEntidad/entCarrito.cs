using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCarrito
    {
        private int idCarrito;
        private entUsuario cliente;
        private entProveedorProducto proveedorProducto;
        private int cantidad;
        private decimal subtotal = 0M;

        public int IdCarrito { get => idCarrito; set => idCarrito = value; }
        
        public entUsuario Cliente { get => cliente; set => cliente = value; }
        
        public entProveedorProducto ProveedorProducto { get => proveedorProducto; set => proveedorProducto = value; }
        
        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser mayor o igual 1")]
        public int Cantidad { get => cantidad; set => cantidad = value; }

        [Required(ErrorMessage = "El subtotal es requerido")]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "SubTotal debe ser mayor a 1")]
        public decimal Subtotal { get => subtotal; set => subtotal = value; }
    }
}
