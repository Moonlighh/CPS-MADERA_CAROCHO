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
using System.Collections;
using System.Security.Cryptography;

namespace CapaAccesoDatos
{
    public class datCarrito
    {
        List<entDetCompra> detCompra = new List<entDetCompra>();//Sirve para guardar temporalmente productos al carrito

        private static readonly datCarrito _instancia = new datCarrito();
        public static datCarrito Instancia
        {
            get { return _instancia; }
        }

        #region Carrito de Compra
        public bool AgregarProductoCarrito(entCarrito car)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spAgregarProductoCarrito", cn);
                cmd.Parameters.AddWithValue("@idCliente", car.Cliente.IdUsuario);
                cmd.Parameters.AddWithValue("@idProveedor_Producto", car.ProveedorProducto.IdProveedorProducto);
                cmd.Parameters.AddWithValue("@cantidad", car.Cantidad);
                cmd.Parameters.AddWithValue("@subTotal", car.Subtotal);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public List<entCarrito> MostrarDetCarrito(int idCliente)
        {
            SqlCommand cmd = null;
            List<entCarrito> list = new List<entCarrito>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spMostrarCarrito", cn);
                cmd.Parameters.AddWithValue("@idUsuario", idCliente);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipoProducto tipoProducto = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString(),
                    };
                    entProveedor proveedor = new entProveedor
                    {
                        RazonSocial = dr["razonSocial"].ToString()
                    };
                    entProducto p = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString().ToUpper(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        Tipo = tipoProducto
                    };
                    entProveedorProducto pro = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                        PrecioCompra = Convert.ToDouble(dr["precioCompra"]),
                        Producto = p,
                        Proveedor = proveedor
                    };
                    entCarrito c = new entCarrito
                    {
                        IdCarrito = Convert.ToInt32(dr["idCarrito"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"]),
                        Subtotal = Convert.ToDouble(dr["subtotal"]),
                        ProveedorProducto = pro
                    };
                    list.Add(c);
                };
            } 
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return list;
        }

        public bool EditarProductoCarrito(entCarrito car)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEditarProductoCarrito", cn);
                cmd.Parameters.AddWithValue("@idCarrito", car.IdCarrito);
                cmd.Parameters.AddWithValue("@cantidad", car.Cantidad);
                cmd.Parameters.AddWithValue("@subtotal", car.Subtotal);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualiza = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return actualiza;
        }
        public bool EliminarProductoCarrito(int idProveedor_Producto, int idCliente)
        {
            SqlCommand cmd = null;
            bool eliminar = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarProductoCarrito", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProveedor_Producto", idProveedor_Producto);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    eliminar = true;
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
            return eliminar;
        }
        #endregion CarritoCompra
    }
}
