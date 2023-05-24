using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoDatos
{
    public interface IDatUsuario
    {
        bool CrearCliente(entUsuario Cli);
        List<entUsuario> ListarUsuarios();
        List<entUsuario> ListarAdministradores();
        List<entUsuario> ListarClientes();
        bool ActualizarCliente(entUsuario Cli);
        bool DeshabilitarUsuario(int id);
        bool HabilitarUsuario(int id);
        entUsuario IniciarSesion(string campo, string contra);
        List<entUsuario> BuscarUsuario(string campo);
        List<entUsuario> BuscarCliente(string busqueda);
        List<entUsuario> BuscarAdministrador(string dato);
        List<entUsuario> OrdenarAdministradores(int orden);
        List<entUsuario> OrdenarClientes(int orden);
        bool CrearSesionUsuario(entUsuario u);
        bool RestablecerPassword(int idUsuario, string correo, string password);
    }
}
