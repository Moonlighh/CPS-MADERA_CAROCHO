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
    public class datAdministrador
    {
        private static readonly datAdministrador instance = new datAdministrador();

        public static datAdministrador Instancia
        {
            get { return instance; }
        }

        #region Metodos
        //Crear Administrador
        public bool CrearAdministrador(entAdministrador admin)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario", admin.Usuario);
                cmd.Parameters.AddWithValue("@contra", admin.Contra);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    creado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error al crear usuario-procedimiento spCrear", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                cmd.Connection.Close();
            }
            return creado;
        }
        //Iniciar Sesion
        public bool IniciarSesion(entAdministrador admin)
        {
            SqlCommand cmd = null;
            bool iniciar = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spIniciarSesion", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@usuario", admin.Usuario);
                cmd.Parameters.AddWithValue("@contra", admin.Contra);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0) {
                    iniciar = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error al crear iniciar sesion -procedimiento spIniciarSesion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return iniciar;
        }
        //Atualizar Usuario
        public bool ActualizarUsuario(entAdministrador admin, string newUsuario)
        {
            SqlCommand cmd = null;
            bool actualizado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarUsuario", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@newUsuario", admin.Usuario);
                cmd.Parameters.AddWithValue("@contra", admin.Contra);
                cmd.Parameters.AddWithValue("@oldUsuario", newUsuario);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualizado = true;
                }

            }
            catch (Exception e)
            {
      
                MessageBox.Show(e.Message, "Error al actualizar usuario -procedimiento spActualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return actualizado;
        }
        //Actualizar Contra
        public bool ActualizarContra(entAdministrador admin, string newContra)
        {
            SqlCommand cmd = null;
            bool actualizado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarContra", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@usuario", admin.Usuario);
                cmd.Parameters.AddWithValue("@oldContra", admin.Contra);
                cmd.Parameters.AddWithValue("@newContra", newContra);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualizado = true;
                }

            }
            catch (Exception e)
            {
      
                MessageBox.Show(e.Message, "Error al crear usuario -procedimiento spActualizarContra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return actualizado;
        }

        #endregion Metodos
    }
}
