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
            return datCliente.Instacia.CrearCliente(c);
        }
        public List<entCliente> ListarCliente()
        {
            return datCliente.Instacia.ListarCliente();
        }
        public bool ActualizarCliente(entCliente c)
        {
            return datCliente.Instacia.ActualizarCliente(c);
        }
        public bool EliminarCliente(int id)
        {
            return datCliente.Instacia.EliminarCliente(id);
        }
        #endregion CRUD

        public List<entCliente> BuscarCliente(string dato)
        {
            return datCliente.Instacia.BuscarCliente(dato);
        }
        public entCliente BuscarIdCliente(int idCliente)
        {
            return datCliente.Instacia.BuscarIdCliente(idCliente);
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
                    u = datCliente.Instacia.IniciarSesion(dato, contra);
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
    }
}
