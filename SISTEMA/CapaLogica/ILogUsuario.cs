using CapaEntidad;
using System.Collections.Generic;

namespace CapaLogica
{
    public interface ILogUsuario
    {
        bool CrearCliente(entUsuario user, out List<string> lsErrores);
        List<entUsuario> ListarUsuarios();
        List<entUsuario> ListarClientes(string dato, string orden);
        List<entUsuario> ListarAdministradores(string dato, string orden);
        bool ActualizarCliente(entUsuario c);
        bool DeshabilitarUsuario(int id);
        bool HabilitarUsuario(int id);
        List<entUsuario> BuscarUsuario(string dato);
        List<entUsuario> BuscarCliente(string dato);
        List<entUsuario> BuscarAdministrador(string dato);
        entUsuario IniciarSesion(string dato, string contra);
        bool CrearSesionUsuario(string usuario, string correo, string password, out List<string> errores);
        bool EnviarCodigoRestablecerPass(string correo);
        bool RestablecerPassword(string password, string codigo);
    }
}

