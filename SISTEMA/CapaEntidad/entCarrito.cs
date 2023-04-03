using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCarrito
    {
        List<entDetVenta> detalle = new List<entDetVenta>();

        public List<entDetVenta> Detalle { get => detalle; set => detalle = value; }

        //private int idDetventa;
        //private entVenta venta;
        //private entProducto producto;
        //private int cantidad;
        //private double subTotal;


        //public void llenarcarrito(entDetVenta vn)
        //{
        //    Detalle.Add(vn);
            
        //}
        //public List<entDetVenta> listarcarrito()
        //{
        //    entDetVenta vn = new  entDetVenta();
        //    for (int i=0;i< detalle.Count();i++)
        //    {
        //        vn = detalle[i];
                
        //    }
        //}
           

    }
}
