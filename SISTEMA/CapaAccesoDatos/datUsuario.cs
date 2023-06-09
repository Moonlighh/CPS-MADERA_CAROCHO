﻿using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace CapaAccesoDatos
{
    public class datUsuario: IDatUsuario
    {

        #region CRUD
        //Crear
        public bool CrearCliente(entUsuario Cli)
        {
            SqlCommand cmd = null;
            bool creado = false;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@razonSocial", Cli.RazonSocial);
                cmd.Parameters.AddWithValue("@dni", Cli.Dni);
                cmd.Parameters.AddWithValue("@telefono", Cli.Telefono);
                cmd.Parameters.AddWithValue("@direccion", Cli.Direccion);
                cmd.Parameters.AddWithValue("@idUbigeo", Cli.Ubigeo.IdUbigeo);
                cmd.Parameters.AddWithValue("@correo", Cli.Correo);
                cmd.Parameters.AddWithValue("@userName", Cli.UserName);
                cmd.Parameters.AddWithValue("@pass", Cli.Pass);
                cmd.Parameters.AddWithValue("@idRol", Cli.Roll.IdRoll);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i >0 )
                    creado = true;
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
        public List<entUsuario> ListarUsuarios()
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarUsuarios", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario Cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonSocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        FechaCreacion =  Convert.ToDateTime(dr["fecCreacion"]),
                    };
                    entUbigeo u = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString(),
                    };
                    entRoll r = new entRoll
                    {
                        Descripcion = dr["descripcion"].ToString()
                    };
                    Cli.Roll = r;
                    Cli.Ubigeo = u;
                    lista.Add(Cli);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Se produjo un error inesperado");

            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }    
        public List<entUsuario> ListarAdministradores()
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarAdministradores", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol
                    };
                    lista.Add(cli);
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
        public List<entUsuario> ListarClientes()
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario Cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        Correo = dr["correo"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"])
                    };
                    entUbigeo u = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString(),
                    };
                    entRoll r = new entRoll
                    {
                        Descripcion = dr["descripcion"].ToString()
                    };
                    Cli.Roll = r;
                    Cli.Ubigeo = u;
                    lista.Add(Cli);
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
        public bool ActualizarCliente(entUsuario Cli)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", Cli.IdUsuario);
                cmd.Parameters.AddWithValue("@razonSocial", Cli.RazonSocial);
                cmd.Parameters.AddWithValue("@dni", Cli.Dni);
                cmd.Parameters.AddWithValue("@telefono", Cli.Telefono);
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
            finally { 
                cmd.Connection.Close(); 
            }
            return actualiza;
        }

        //Eliminar - Activo
        public bool DeshabilitarUsuario(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spDeshabilitarUsuario", cn);
                cmd.Parameters.AddWithValue("@idUsuario", id);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public bool HabilitarUsuario(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spHabilitarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", id);
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

        #region OTROS
        public entUsuario IniciarSesion(string campo, string contra)
        {
            SqlCommand cmd = null;
            entUsuario c = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spIniciarSesion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dato", campo);
                cmd.Parameters.AddWithValue("@contra", contra);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        c = new entUsuario
                        {
                            IdUsuario = Convert.ToInt16(dr["idUsuario"]),
                            UserName = dr["userName"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Rol = (entRol)dr["idRol"],//Convertir (castearlo) a objeto de tipo entRol
                            Activo = Convert.ToBoolean(dr["activo"]),                       
                            RazonSocial = dr["razonSocial"].ToString(),
                            Dni = dr["dni"].ToString(),
                            Telefono = dr["telefono"].ToString(),
                            Direccion = dr["direccion"].ToString(),         

                        };
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { 
                cmd.Connection.Close(); 
            }
            return c;
        }
        public List<entUsuario> BuscarAdministrador(string dato)
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarAdministrador", cn);
                cmd.Parameters.AddWithValue("@Campo", dato);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol
                    };
                    lista.Add(cli);
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
        public List<entUsuario> BuscarCliente(string busqueda)
        {
            List<entUsuario> lista = new List<entUsuario>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarCliente", cn);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda );
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario Cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        Correo = dr["correo"].ToString(),
                        UserName = dr["userName"].ToString(),
                    };
                    entUbigeo u = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString(),
                    };
                    entRoll r = new entRoll
                    {
                        Descripcion = dr["descripcion"].ToString()
                    };
                    Cli.Roll = r;
                    Cli.Ubigeo = u;
                    lista.Add(Cli);
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
        public List<entUsuario> BuscarUsuario(string campo)
        {
            List<entUsuario> lista = new List<entUsuario>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@campo", campo);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario user = new entUsuario
                    {
                        IdUsuario = Convert.ToInt16(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        FechaCreacion = Convert.ToDateTime(dr["fecCreacion"])
                    };
                    entUbigeo u = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString(),
                    };                                                                                                                          
                    entRoll r = new entRoll
                    {
                        Descripcion = dr["descripcion"].ToString()
                    };
                    user.Roll = r;
                    user.Ubigeo = u;
                    lista.Add(user);
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
        public List<entUsuario> OrdenarAdministradores(int orden)
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spOrdenarAdministrador", cn);
                cmd.Parameters.AddWithValue("@orden", orden);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol
                    };
                    lista.Add(cli);
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
        public List<entUsuario> OrdenarClientes(int orden)
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spOrdenarClientes", cn);
                cmd.Parameters.AddWithValue("@orden", orden);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario cli = new entUsuario
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol
                    };
                    lista.Add(cli);
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
        public bool CrearSesionUsuario(entUsuario u)
        {
            bool creado = false;

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand("spCrearSesionUsuario", cn))
                    {
                        cmd.Parameters.AddWithValue("@userName", u.UserName);
                        cmd.Parameters.AddWithValue("@correo", u.Correo);
                        cmd.Parameters.AddWithValue("@pass", u.Pass);
                        cmd.Parameters.AddWithValue("@idRol", u.Roll.IdRoll);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();
                        creado = cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("UQ_USUARIO_USERNAME"))
                {
                    throw new Exception($"El nombre de usuario {u.UserName} ya esta asociado a una cuenta existente");
                }
                if (ex.Message.ToUpper().Contains("CHK_USUARIO_USERNAME"))
                {
                    throw new Exception($"El nombre de usuario {u.UserName} no cumple con el formato establecido");
                }
                if (ex.Message.ToUpper().Contains("CHK_USUARIO_CORREO"))
                {
                    throw new Exception($"El correo {u.Correo} no cumple con el formato establecido");
                }
                if (ex.Message.ToUpper().Contains("UQ_USUARIO_CORREO"))
                {
                    throw new Exception($"El correo {u.Correo} ya esta asociado a una cuenta existente");
                }
            }
            return creado;
        }
        public bool RestablecerPassword(int idUsuario, string pass, string correo)
        {
            bool restablecer = false;
            try
            {
                using (var cn = Conexion.Instancia.Conectar())
                {
                    using (var cmd = new SqlCommand("spRestablecerPassword", cn))
                    {
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario );
                        cmd.Parameters.AddWithValue("@pass", pass);
                        cmd.Parameters.AddWithValue("@correo", correo );
                        cmd.CommandType = CommandType.StoredProcedure;
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        restablecer = i> 0;
                    }
                }
            }
            catch (Exception e)
            {

                throw new Exception($"Se produjo el siguiente error {e.Message}");
            }
            return restablecer;
        }
    }
    #endregion OTROS
}
