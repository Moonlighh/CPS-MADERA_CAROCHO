using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
   public class entReporteVenta
    {
        int codigo;
        string cliente;
        DateTime fecha;
        string descripcion;
        string longitud;
        string cantidad;
        string precUnitario;
        double subTotal;

        #region Get and Set
        public int Codigo { get => codigo; set => codigo = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Longitud { get => longitud; set => longitud = value; }
        public string Cantidad { get => cantidad; set => cantidad = value; }
        public string PrecUnitario { get => precUnitario; set => precUnitario = value; }
        public double SubTotal { get => subTotal; set => subTotal = value; }
        #endregion
    }
}
