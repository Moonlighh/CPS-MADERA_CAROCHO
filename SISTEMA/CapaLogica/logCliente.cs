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
    public class logCliente
    {
        private static readonly logCliente _instancia = new logCliente();
        public static logCliente Instancia
        {
            get { return _instancia; }
        }
        #region CRUD
        public bool CrearCliente(entCliente c)
        {
            if (string.IsNullOrEmpty(c.UserName) || string.IsNullOrEmpty(c.Correo))
            {
                return false;
            }
            c.Pass = logRecursos.GetSHA256(c.Pass);
            return datCliente.Instancia.CrearCliente(c);
        }
        public List<entCliente> ListarCliente()
        {
            return datCliente.Instancia.ListarCliente();
        }
        public bool ActualizarCliente(entCliente c)
        {
            return datCliente.Instancia.ActualizarCliente(c);
        }
        public bool EliminarCliente(int id)
        {
            return datCliente.Instancia.EliminarCliente(id);
        }
        #endregion CRUD

        public List<entCliente> BuscarCliente(string dato)
        {
            return datCliente.Instancia.BuscarCliente(dato);
        }
        public entCliente BuscarIdCliente(int idCliente)
        {
            return datCliente.Instancia.BuscarIdCliente(idCliente);
        }

        public entCliente IniciarSesion(string dato, string contra)
        {
            entCliente u = null;
            try
            {
                if (DateTime.Now.Hour > 24)
                {
                    throw new ApplicationException("No puede ingresar a esta hora");
                }
                else
                {
                    contra = logRecursos.GetSHA256(contra);
                    u = datCliente.Instancia.IniciarSesion(dato, contra);
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
        public List<entCliente> BuscarUsuarioAdmin(string dato)
        {
            return datCliente.Instancia.BuscarUsuarioAdmin(dato);
        }
        public List<entCliente> ListarUsuarioAdmin()
        {
            return datCliente.Instancia.ListarUsuarioAdmin();
        }
    }
}
