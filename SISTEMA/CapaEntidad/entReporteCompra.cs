using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entReporteCompra
    {
        int codigo;
        string proveedor;
        DateTime fecha;
        string descripcion;
        string longitud;
        string cantidad;
        string precUnitario;
        double subTotal;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Proveedor { get => proveedor; set => proveedor = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Longitud { get => longitud; set => longitud = value; }
        public string Cantidad { get => cantidad; set => cantidad = value; }
        public string PrecUnitario { get => precUnitario; set => precUnitario = value; }
        public double SubTotal { get => subTotal; set => subTotal = value; }
    }
}
