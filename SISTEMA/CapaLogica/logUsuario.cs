using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaLogica
{
    public class logUsuario
    {
        private static readonly logUsuario _instancia = new logUsuario();
        public static logUsuario Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public bool CrearCliente(entUsuario c)
        {
            if (string.IsNullOrEmpty(c.UserName) || string.IsNullOrEmpty(c.Correo))
            {
                return false;
            }
            c.Pass = logRecursos.GetSHA256(c.Pass);
            return datUsuario.Instancia.CrearCliente(c);
        }
        public List<entUsuario> ListarUsuarios()
        {
            return datUsuario.Instancia.ListarUsuarios();
        }
        public List<entUsuario> ListarClientes()
        {
            return datUsuario.Instancia.ListarClientes();
        }
        public bool ActualizarCliente(entUsuario c)
        {
            return datUsuario.Instancia.ActualizarCliente(c);
        }
        public bool DeshabilitarUsuario(int id)
        {
            return datUsuario.Instancia.DeshabilitarUsuario(id);
        }
        public bool HabilitarUsuario(int id)
        {
            return datUsuario.Instancia.HabilitarUsuario(id);
        }
        #endregion

        #region Otros
        public List<entUsuario> BuscarUsuario(string dato)
        {
            return datUsuario.Instancia.BuscarUsuario(dato);
        }
        public List<entUsuario> BuscarCliente(string dato)
        {
            return datUsuario.Instancia.BuscarCliente(dato);
        }
        public List<entUsuario> BuscarAdministrador(string dato)
        {
            return datUsuario.Instancia.BuscarAdministrador(dato);
        }



        public entUsuario BuscarIdCliente(int idUsuario)
        {
            return datUsuario.Instancia.BuscarIdCliente(idUsuario);
        }

        public entUsuario IniciarSesion(string dato, string contra)
        {
            entUsuario u = null;
            try
            {
                if (DateTime.Now.Hour > 24)
                {
                    throw new ApplicationException("No puede ingresar a esta hora");
                }
                else
                {
                    contra = logRecursos.GetSHA256(contra);
                    u = datUsuario.Instancia.IniciarSesion(dato, contra);
                    if (u != null)
                    {
                        if (!u.Activo)
                        {
                            return u = null;
                            throw new ApplicationException("Usuario ha sido dado de baja");
                        }

                    }
                    else
                    {
                        throw new ApplicationException("Datos invalidos");
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
            return u;
        }
        public List<entUsuario> ListarAdministradores()
        {
            return datUsuario.Instancia.ListarAdministradores();
        }
        #endregion
    }
}
