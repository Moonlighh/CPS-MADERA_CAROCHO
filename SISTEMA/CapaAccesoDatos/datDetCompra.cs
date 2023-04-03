using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CapaAccesoDatos
{
    public class datDetCompra
    {
        List<entDetCompra> detCompra = new List<entDetCompra>();//Sirve para guardar temporalmente productos al carrito
        private static readonly datDetCompra _instancia = new datDetCompra();
        public static datDetCompra Instancia
        {
            get { return _instancia; }
        }

        //Cada Compra tiene su Detalle
        public bool CrearDetCompra(entDetCompra Det)
        {

            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearDetCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompra", Det.Compra.IdCompra);
                cmd.Parameters.AddWithValue("@idProducto", Det.Producto.IdProducto);//revisar si se llena el obj completo
                cmd.Parameters.AddWithValue("@cantidad", Det.Cantidad);
                cmd.Parameters.AddWithValue("@subTotal", Det.Subtotal);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    creado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return creado;
        }

        #region CarritoCompra
        public void AgregarProductoCarrito(entDetCompra det)
        {
            detCompra.Add(det);
        }

        public List<entDetCompra> MostrarDetCarrito()
        {
            var lista = new List<entDetCompra>();
            for (int i = 0; i < detCompra.Count(); i++)
            {
                entDetCompra dv = new entDetCompra();
                dv = detCompra[i];//Obs
                lista.Add(dv);
            }
            return lista;
        }

        public bool EliminarDetCarrito(int id)
        {
            bool eliminado = false;
            try
            {
                for (int i = 0; i < detCompra.Count(); i++)
                {
                    if (detCompra[i].Producto.IdProducto.Equals(id))
                    {
                        detCompra.RemoveAt(i);
                    }
                }
                eliminado= true;
            }
            catch (Exception)
            {
                MessageBox.Show("El elemento esta fuera del indice o no existe", "Maderera carocho", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return eliminado;

        }
        #endregion CarritoCompra

        public List<entReporteCompra> MostrarReporteCompra(int idCompra)
        {

            SqlCommand cmd = null;
            var lista = new List<entReporteCompra>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spMostrarReporteCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompra", idCompra);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entReporteCompra rpCompra = new entReporteCompra();

                    rpCompra.Codigo = Convert.ToInt32(dr["CODIGO"]);
                    rpCompra.Proveedor = dr["PROVEEDOR"].ToString().ToUpper();
                    rpCompra.Fecha = Convert.ToDateTime(dr["FECHA"]);
                    rpCompra.Descripcion = dr["DESCRIPCIÓN"].ToString().ToUpper();
                    rpCompra.Longitud = dr["LONGITUD"].ToString();
                    rpCompra.Cantidad = dr["CANTIDAD"].ToString();
                    rpCompra.PrecUnitario = dr["PRECIO_UNITARIO"].ToString();
                    rpCompra.SubTotal = Convert.ToDouble(dr["SUBTOTAL"]);

                    lista.Add(rpCompra);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
    }
}
