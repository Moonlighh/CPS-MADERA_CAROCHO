using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaAccesoDatos
{
    public class datProveedorProducto
    {
        private static datProveedorProducto _instancia = new datProveedorProducto();

        public static datProveedorProducto Instancia
        {
            get { return _instancia; }
        }

        public List<entProveedorProducto> ListarProveedorProducto()
        {
            SqlCommand cmd = null;
            List<entProveedorProducto> lista = new List<entProveedorProducto>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarProveedorProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor p = new entProveedor
                    {
                        IdProveedor = Convert.ToInt32(dr["idProveedor"]),
                        RazonSocial = dr["proveedor"].ToString(),
                    };
                    entProducto prod = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["madera"].ToString(),
                    };
                    entProveedorProducto obj = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["id"]),
                        Proveedor = p,
                        Producto = prod,
                        PrecioCompra = Convert.ToDouble(dr["precio"])
                    };
                    lista.Add(obj);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR AL MOSTRAR DATOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
    }
}
