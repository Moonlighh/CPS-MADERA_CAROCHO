﻿using System;
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
        private double subtotal;

        public int IdCarrito { get => idCarrito; set => idCarrito = value; }
        
        public entUsuario Cliente { get => cliente; set => cliente = value; }
        
        public entProveedorProducto ProveedorProducto { get => proveedorProducto; set => proveedorProducto = value; }
        
        [Required(ErrorMessage = "La cantidad es requerida.")]
        public int Cantidad { get => cantidad; set => cantidad = value; }

        [Required(ErrorMessage = "El subtotal es requerido")]
        public double Subtotal { get => subtotal; set => subtotal = value; }
    }
}
