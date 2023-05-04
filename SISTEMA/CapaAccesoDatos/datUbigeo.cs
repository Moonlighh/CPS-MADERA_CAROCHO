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
    public class datUbigeo
    {
        private static readonly datUbigeo _instancia = new datUbigeo();
        public static datUbigeo Instancia
        {
            get { return _instancia; }
        }
        public bool CrearUbigeo(entUbigeo ubi)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearUbigeo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUbigeo", ubi.IdUbigeo);
                cmd.Parameters.AddWithValue("@departamento", ubi.Departamento);
                cmd.Parameters.AddWithValue("@provincia", ubi.Provincia);
                cmd.Parameters.AddWithValue("@distrito", ubi.Distrito);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
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
        //Leer
        public List<entUbigeo> ListarUbigeo()
        {
            SqlCommand cmd = null;
            List<entUbigeo> lista = new List<entUbigeo>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarUbigeo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUbigeo ubi = new entUbigeo();
                    ubi.IdUbigeo = dr["idUbigeo"].ToString();
                    ubi.Departamento = dr["departamento"].ToString();
                    ubi.Provincia =dr["provincia"].ToString();
                    ubi.Distrito =dr["distrito"].ToString();
                    lista.Add(ubi);
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

        public List<entUbigeo> ListarDistrito()
        {
            SqlCommand cmd = null;
            List<entUbigeo> lista = new List<entUbigeo>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_ListaDistrito", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUbigeo ubi = new entUbigeo();
                    ubi.IdUbigeo = dr["idUbigeo"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    lista.Add(ubi);
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

        public bool ActualizarUbigeo(entUbigeo ubi)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarUbigeo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUbigeo", ubi.IdUbigeo);
                cmd.Parameters.AddWithValue("@departamento", ubi.Departamento);
                cmd.Parameters.AddWithValue("@provincia", ubi.Provincia);
                cmd.Parameters.AddWithValue("@distrito", ubi.Distrito);
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
        public bool EliminarUbigeo(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarUbigeo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUbigeo", id);
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

        public List<entUbigeo> BuscarUbigeo(string busqueda)
        {
            List<entUbigeo> lista = new List<entUbigeo>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarUbigeo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUbigeo ubi = new entUbigeo();
                    ubi.IdUbigeo = dr["idUbigeo"].ToString();
                    ubi.Departamento = dr["departamento"].ToString();
                    ubi.Provincia =dr["provincia"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    lista.Add(ubi);
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
