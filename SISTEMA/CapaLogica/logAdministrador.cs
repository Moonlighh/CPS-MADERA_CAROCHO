using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logAdministrador
    {
        private static readonly logAdministrador _instancia = new logAdministrador();
        public static logAdministrador Instancia
        {
            get { return _instancia; }
        }
        public bool CrearAdministrador(entAdministrador admin)
        {
            return datAdministrador.Instancia.CrearAdministrador(admin);
        }
        public bool IniciarSesion(entAdministrador admin)
        {
            return datAdministrador.Instancia.IniciarSesion(admin);
        }
        public bool ActualizarUsuario(entAdministrador admin, string newUsuario)
        {
            return datAdministrador.Instancia.ActualizarUsuario(admin, newUsuario);
        }
        public bool ActualizarContra(entAdministrador admin, string newContra)
        {
            return datAdministrador.Instancia.ActualizarContra(admin, newContra);
        }
    }
}
