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
    public class datUsuario
    {
        #region singleton
        private static readonly datUsuario _instancia = new datUsuario();
        public static datUsuario Instancia
        {
            get { return datUsuario._instancia; }
        }
        #endregion singleton

        #region Metodos
        public entUsuario IniciarSesion(string campo, string contra)
        {
            SqlCommand cmd = null;
            entUsuario u = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spIniciarSesion", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dato", campo);
                cmd.Parameters.AddWithValue("@contra", encrypt.GetSHA256(contra));
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        u = new entUsuario
                        {
                            IdUsuario = Convert.ToInt16(dr["idUsuario"]),
                            UserName = dr["userName"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Rol = (entRol) dr["idRol"],//Convertir (castearlo) a objeto de tipo entRol
                            Activo = Convert.ToBoolean(dr["activo"])
                        };
                        entCliente c = new entCliente
                        {
                            RazonSocial = dr["razonSocial"].ToString(),
                            Dni = dr["dni"].ToString(),
                            Telefono = dr["telefono"].ToString(),
                            Direccion = dr["direccion"].ToString(),
                            IdCliente = Convert.ToInt32(dr["idCliente"])

                    };
                        u.Cliente = c;
                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return u;
        }
        public bool CrearUsuario(entUsuario usu)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", usu.UserName);
                cmd.Parameters.AddWithValue("@correo", usu.Correo);
                cmd.Parameters.AddWithValue("@password", usu.Pass);
                cmd.Parameters.AddWithValue("@idRol", usu.Roll.Idrol);
                cmd.Parameters.AddWithValue("@cliente", usu.Cliente.IdCliente);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    creado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "ERROR AL INSERTAR UN USUARIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return creado;

        }
        public List<entUsuario> ObtenerUsuarios()
        {
            List<entUsuario> rptListaUsuario = new List<entUsuario>();
            using (SqlConnection oConexion = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spObtenerUsuario", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUsuario.Add(new entUsuario()
                        {
                            IdUsuario = Convert.ToInt16(dr["idUsuario"]),
                            UserName = dr["userName"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Pass = dr["pass"].ToString(),

                        });
                    }
                    dr.Close();

                    return rptListaUsuario;

                }
                catch (Exception ex)
                {
                    rptListaUsuario = null;
                    return rptListaUsuario;
                }
            }
        }

        public List<entUsuario> ListarUsuarioAdmin()
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarClienteAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    entCliente cli = new entCliente
                    {
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString()
                    };
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario usu = new entUsuario()
                    {
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol,
                        Cliente = cli,


                    };

                    lista.Add(usu);
                }
            }
            catch ( Exception e)
            {
                MessageBox.Show(e.Message, "ERROR AL MOSTRAR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        //Eliminar - Deshabilitar
        public bool EliminarUsuario(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario", id);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    eliminado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return eliminado;
        }

        public List<entUsuario>BuscarUsuarioAdmin(string dato)
        {
            SqlCommand cmd = null;
            List<entUsuario> lista = new List<entUsuario>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarClienteAdmin", cn);
                cmd.Parameters.AddWithValue("@Campo", dato);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    entCliente cli = new entCliente
                    {
                        IdCliente = Convert.ToInt32(dr["idCliente"]),
                        RazonSocial = dr["razonsocial"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString()
                    };
                    entRoll rol = new entRoll()
                    {
                        Descripcion = dr["descripcion"].ToString(),
                    };
                    entUsuario usu = new entUsuario()
                    {
                        UserName = dr["userName"].ToString(),
                        Correo = dr["correo"].ToString(),
                        Activo = Convert.ToBoolean(dr["activo"]),
                        Roll = rol,
                        Cliente = cli,


                    };

                    lista.Add(usu);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR AL buscaar", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }




        #endregion Metodos
    }
}
