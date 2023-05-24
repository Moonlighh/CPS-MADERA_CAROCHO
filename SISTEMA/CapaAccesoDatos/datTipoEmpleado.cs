using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
namespace CapaAccesoDatos
{
    public class datTipoEmpleado
    {
        private static readonly datTipoEmpleado _instancia = new datTipoEmpleado();
        public static datTipoEmpleado Instancia
        {
            get { return _instancia; }
        }
        #region CRUD
        //Crear
        public bool CrearTipoEmpleado(entTipoEmpleado tip)
        {
            bool creado = false;
            try
            {
                using (var cn = Conexion.Instancia.Conectar())
                {
                    using (var cmd = new SqlCommand("spCrearTipoEmpleado", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tipoEmpleado", tip.Nombre);
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        creado = i> 0;
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
        public List<entTipoEmpleado> ListarTipoEmpleado()
        {
            SqlCommand cmd = null;
            List<entTipoEmpleado> lista = new List<entTipoEmpleado>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarTipoEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipoEmpleado tip = new entTipoEmpleado();
                    tip.IdTipo_Empleado = Convert.ToInt32(dr["idTipo_Empleado"]);
                    tip.Nombre = dr["nombre"].ToString();
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

        public bool ActualizarTipoEmpleado(entTipoEmpleado tip)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarTipoEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTipoEmpleado", tip.IdTipo_Empleado);
                cmd.Parameters.AddWithValue("@nombre", tip.Nombre);
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
        public bool EliminarTipoEmpleado(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarTipoEmpleado", cn);
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
        #endregion CRUD
    }
}
