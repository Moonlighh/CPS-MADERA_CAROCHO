using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaAccesoDatos
{
    public class datDetVenta
    {
        List<entDetVenta> detVenta = new List<entDetVenta>();//Sirve para guardar temporalmente productos al carrito

        private static readonly datDetVenta _instancia = new datDetVenta();
        public static datDetVenta Instancia
        {
            get { return _instancia; }
        }

        //Cada Venta tiene su Detalle
        public bool CrearDetVenta(entDetVenta Det, int idVenta)
        {

            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearDetVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cmd.Parameters.AddWithValue("@idProducto", Det.Producto.IdProducto);//revisar si se llena el obj completo
                cmd.Parameters.AddWithValue("@cantidad", Det.Cantidad);
                cmd.Parameters.AddWithValue("@subTotal", Det.Subtotal);
                cn.Open();

                // Al ejecutar el procedimiento se inserta el detalle y se actualiza el stock del producto
                int i = cmd.ExecuteNonQuery();// Debe dar 2 filas afectadas
                if (i != 0)
                {
                    creado = true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return creado;
        }

        // Mostrar el detalle por cada usuario
        public List<entDetVenta> MostrarDetalleVenta(int idUsuario, int idVenta)
        {

            SqlCommand cmd = null;
            var lista = new List<entDetVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spMostrarDetalleVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entDetVenta rpVenta = new entDetVenta();
                    entProducto producto = new entProducto();
                    entVenta Venta = new entVenta();
                    Venta.IdVenta = Convert.ToInt32(dr["idVenta"]);
                    producto.Nombre = dr["nombre"].ToString().ToUpper();
                    producto.Longitud = Convert.ToDouble(dr["longitud"]);
                    producto.Diametro = Convert.ToDouble(dr["diametro"]);
                    rpVenta.Cantidad = Convert.ToInt32(dr["cantidad"]);
                    rpVenta.Subtotal = Convert.ToDecimal(dr["subtotal"]);

                    rpVenta.Producto = producto;
                    rpVenta.Venta = Venta;
                    lista.Add(rpVenta);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
    }
}
