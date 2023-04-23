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

        #region Carrito de Compra
        public void AgregarProductoCarrito(entDetCompra det)
        {
            detCompra.Add(det);
        }

        public List<entDetCompra> MostrarDetCarrito()
        {
            return detCompra;
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
                eliminado = true;
            }
            catch (Exception)
            {
                MessageBox.Show("El elemento esta fuera del indice o no existe", "Maderera carocho", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return eliminado;

        }
        #endregion CarritoCompra

        public List<entDetCompra> MostrarDetalleCompra(int idCompra)
        {

            SqlCommand cmd = null;
            var lista = new List<entDetCompra>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spMostrarDetalleCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompra", idCompra);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entDetCompra rpCompra = new entDetCompra();
                    entProducto producto = new entProducto();
                    entCompra compra = new entCompra();
                    compra.IdCompra = Convert.ToInt32(dr["idCompra"]);
                    producto.Nombre = dr["nombre"].ToString().ToUpper();
                    producto.Longitud = Convert.ToDouble(dr["longitud"]);
                    producto.Diametro = Convert.ToDouble(dr["diametro"]);
                    rpCompra.Cantidad = Convert.ToInt32(dr["cantidad"]);
                    rpCompra.Subtotal = Convert.ToDouble(dr["subtotal"]);

                    rpCompra.Producto = producto;
                    rpCompra.Compra = compra;
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
