using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
namespace CapaAccesoDatos
{
    public class datTipoProducto
    {
        private static readonly datTipoProducto _instancia = new datTipoProducto();

        public static datTipoProducto Instancia
        {
            get { return _instancia; }
        }

        public bool CrearTipoProducto(entTipoProducto tip)
        {
            bool creado = false;
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    using (var cmd = new SqlCommand("spCrearTipoProducto", cn))
                    {
                        cmd.Parameters.AddWithValue("@tipo", tip.Tipo);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();
                        creado = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return creado;
        }
        
        //Leer
        public List<entTipoProducto> ListarTipoProducto()
        {
            SqlCommand cmd = null;
            List<entTipoProducto> lista = new List<entTipoProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarTipoProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipoProducto tip = new entTipoProducto();
                    tip.IdTipo_producto = Convert.ToInt32(dr["idTipo_Producto"]);
                    tip.Tipo = dr["tipo"].ToString();
                    lista.Add(tip);
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
       
        //Actualizar
        public bool ActualizarTipoProducto(entTipoProducto tip)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarTipoProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTipo_Producto", tip.IdTipo_producto);
                cmd.Parameters.AddWithValue("@tipo", tip.Tipo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualiza = true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return actualiza;
        }

        //Eliminar - Deshabilitar
        public bool EliminarTipoProducto(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarTipoProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTipo_Empleado", id);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    eliminado = true;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return eliminado;
        }
    }
}
