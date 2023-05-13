using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace CapaAccesoDatos
{
    public class datProveedor
    {
        private static readonly datProveedor _instance = new datProveedor();
        public static datProveedor Instancia
        {
            get { return _instance; }
        }

        #region CRUD
        //Crear
        public bool CrearProveedor(entProveedor pro)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@razonSocial", pro.RazonSocial);
                cmd.Parameters.AddWithValue("@dni", pro.Dni);
                cmd.Parameters.AddWithValue("@correo", pro.Correo);
                cmd.Parameters.AddWithValue("@telefono", pro.Telefono);
                cmd.Parameters.AddWithValue("@descripcion", pro.Descripcion);
                cmd.Parameters.AddWithValue("@estProveedor", pro.EstProveedor);
                cmd.Parameters.AddWithValue("@idUbigeo", pro.Ubigeo.IdUbigeo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
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
        public List<entProveedor> ListarProveedor()
        {
            SqlCommand cmd = null;
            List<entProveedor> list = new List<entProveedor>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor pro = new entProveedor();
                    entUbigeo ubi = new entUbigeo();
                    pro.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    pro.RazonSocial = dr["razonSocial"].ToString();
                    pro.Dni = dr["dni"].ToString();
                    pro.Correo = dr["correo"].ToString();
                    pro.Telefono = dr["telefono"].ToString();
                    pro.Descripcion = dr["descripcion"].ToString();
                    pro.EstProveedor = Convert.ToBoolean(dr["estProveedor"]);
                    ubi.Departamento = dr["departamento"].ToString();
                    ubi.Provincia = dr["provincia"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    pro.Ubigeo = ubi;
                    list.Add(pro);
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
            return list;
        }
        //Actualizar
        public bool ActualizarProveedor(entProveedor pro)
        {
            SqlCommand cmd = null;
            bool actualizado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProveedor", pro.IdProveedor);
                cmd.Parameters.AddWithValue("@razonSocial", pro.RazonSocial);
                cmd.Parameters.AddWithValue("@dni", pro.Dni);
                cmd.Parameters.AddWithValue("@correo", pro.Correo);
                cmd.Parameters.AddWithValue("@telefono", pro.Telefono);
                cmd.Parameters.AddWithValue("@descripcion", pro.Descripcion);
                cmd.Parameters.AddWithValue("@estProveedor", pro.EstProveedor);
                cmd.Parameters.AddWithValue("@idUbigeo", pro.Ubigeo.IdUbigeo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualizado = true;
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
            return actualizado;
        }
        //Eliminar
        public bool DeshabilitarProveedor(int idProveedor)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
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
            finally
            {
                cmd.Connection.Close();
            }
            return eliminado;
        }
        #endregion CRUD

        #region OTROS
        public List<entProveedor> BuscarProveedor(string busqueda)
        {
            List<entProveedor> list = new List<entProveedor>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor pro = new entProveedor();
                    pro.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    pro.RazonSocial = dr["razonSocial"].ToString();
                    pro.Dni = dr["dni"].ToString();
                    pro.Correo = dr["correo"].ToString();
                    pro.Telefono = dr["telefono"].ToString();
                    pro.Descripcion = dr["descripcion"].ToString();
                    pro.EstProveedor = Convert.ToBoolean(dr["estProveedor"]);
                    entUbigeo ubi = new entUbigeo();
                    ubi.Departamento = dr["departamento"].ToString();
                    ubi.Provincia = dr["provincia"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    pro.Ubigeo = ubi;
                    list.Add(pro);
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
            return list;
        }
        public entProveedor BuscarIdProveedor(int idProveedor)
        {
            SqlCommand cmd = null;
            entProveedor pro = new entProveedor();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarIdProveedor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pro.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    pro.RazonSocial = dr["razonSocial"].ToString();
                    pro.Dni = dr["dni"].ToString();
                    pro.Correo = dr["correo"].ToString();
                    pro.Telefono = dr["telefono"].ToString();
                    pro.Descripcion = dr["descripcion"].ToString();
                    pro.EstProveedor = Convert.ToBoolean(dr["estProveedor"]);
                    entUbigeo ubi = new entUbigeo();
                    ubi.IdUbigeo = dr["idUbigeo"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    pro.Ubigeo = ubi;
                }
            }
            catch (Exception e)
            { throw e; }
            finally { cmd.Connection.Close(); }
            return pro;
        }
        public List<entProveedor> OrdenarProveedores(int orden)
        {
            SqlCommand cmd = null;
            List<entProveedor> list = new List<entProveedor>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spOrdenarProveedor", cn);
                cmd.Parameters.AddWithValue("@orden", orden);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor pro = new entProveedor();
                    entUbigeo ubi = new entUbigeo();
                    pro.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    pro.RazonSocial = dr["razonSocial"].ToString();
                    pro.Dni = dr["dni"].ToString();
                    pro.Correo = dr["correo"].ToString();
                    pro.Telefono = dr["telefono"].ToString();
                    pro.Descripcion = dr["descripcion"].ToString();
                    pro.EstProveedor = Convert.ToBoolean(dr["estProveedor"]);
                    ubi.Departamento = dr["departamento"].ToString();
                    ubi.Provincia = dr["provincia"].ToString();
                    ubi.Distrito = dr["distrito"].ToString();
                    pro.Ubigeo = ubi;
                    list.Add(pro);
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
            return list;
        }
        #endregion
    }
}
