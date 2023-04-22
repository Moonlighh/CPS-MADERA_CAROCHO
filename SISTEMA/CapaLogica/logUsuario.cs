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
        public List<entUsuario> ListarCliente()
        {
            return datUsuario.Instancia.ListarCliente();
        }
        public bool ActualizarCliente(entUsuario c)
        {
            return datUsuario.Instancia.ActualizarCliente(c);
        }
        public bool DeshabilitarUsuario(int id)
        {
            return datUsuario.Instancia.DeshabilitarUsuario(id);
        }
        #endregion

        #region Otros
        public List<entUsuario> BuscarCliente(string dato)
        {
            return datUsuario.Instancia.BuscarCliente(dato);
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
        public List<entUsuario> BuscarUsuarioAdmin(string dato)
        {
            return datUsuario.Instancia.BuscarUsuarioAdmin(dato);
        }
        public List<entUsuario> ListarUsuarioAdmin()
        {
            return datUsuario.Instancia.ListarUsuarioAdmin();
        }
        #endregion
    }
}
