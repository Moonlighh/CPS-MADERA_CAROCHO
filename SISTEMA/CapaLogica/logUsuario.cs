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
        #region singleton
        private static readonly logUsuario _instancia = new logUsuario();
        public static logUsuario Instancia
        {
            get { return logUsuario._instancia; }
        }
        #endregion singleton

        #region Metodos
        public bool CrearUsuario(entUsuario usu)
        {
            return datUsuario.Instancia.CrearUsuario(usu);
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
        public List<entUsuario> ObtenerUsuarios()
        {
            return datUsuario.Instancia.ObtenerUsuarios();
        }
        
        public List<entUsuario> ListarUsuarioAdmin()
        {
            return datUsuario.Instancia.ListarUsuarioAdmin();
        }

        public List<entUsuario>BuscarUsuarioAdmin(string dato)
        {
            return datUsuario.Instancia.BuscarUsuarioAdmin(dato);
        }
        public bool EliminarUsuaro(int id)
        {
            return datUsuario.Instancia.EliminarUsuario(id);
        }
        #endregion Metodos
    }
}
