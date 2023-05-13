using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;//Validaciones

namespace CapaLogica
{
    public class logUsuario: ILogUsuario
    {
        /*
         * Se está implementando la técnica de inyección de dependencias, que tiene
         * como objetivo reducir el acoplamiento entre las diferentes capas de la aplicación.    
         */
        private readonly IDatUsuario _datUsuario;
        public logUsuario(IDatUsuario datUsuario)
        {
            /* Este constructor está permitiendo que la clase logUsuario reciba una instancia
             * de la capa de acceso a datos a través de su constructor, en lugar de crearla
             * directamente dentro de la clase, lo que permite una mayor flexibilidad y facilita
             * la prueba.
             */
            _datUsuario = datUsuario;
        }

        #region CRUD
        public bool CrearCliente(entUsuario user, out List<string> lsErrores)
        {
            bool isValid = ValidationHelper.TryValidateEntityMsj(user, out lsErrores);
            if (!isValid)
            {
                return false;
            }
            user.Pass = logRecursos.GetSHA256(user.Pass);
            return _datUsuario.CrearCliente(user);
        }
        public List<entUsuario> ListarUsuarios()
        {
            return _datUsuario.ListarUsuarios();
        }
        // Método que devuelve una lista de entidades "Usuario" (clientes).
        // El parámetro "dato" se utiliza para buscar clientes por su nombre o correo electrónico.
        // El parámetro "orden" se utiliza para especificar la dirección de ordenamiento: "asc" para ascendente y "desc" para descendente.
        public List<entUsuario> ListarClientes(string dato, string orden)
        {
            // Si el parámetro "dato" no está vacío, buscar clientes por su nombre o correo electrónico.
            if (!string.IsNullOrEmpty(dato))
            {
                return _datUsuario.BuscarCliente(dato);
            }

            // Si el parámetro "orden" está vacío, devolver la lista de clientes sin ordenar.
            if (string.IsNullOrEmpty(orden))
            {
                return _datUsuario.ListarClientes();
            }

            // Determinar la dirección de ordenamiento.
            bool ordenAscendente = (orden.ToLower() == "asc");

            // Llamar al método "OrdenarClientes()" con un valor entero (1 para ascendente, 0 para descendente).
            return _datUsuario.OrdenarClientes(ordenAscendente ? 1 : 0);
        }

        public bool ActualizarCliente(entUsuario c)
        {
            return _datUsuario.ActualizarCliente(c);
        }
        public bool DeshabilitarUsuario(int id)
        {
            return _datUsuario.DeshabilitarUsuario(id);
        }
        public bool HabilitarUsuario(int id)
        {
            return _datUsuario.HabilitarUsuario(id);
        }
        #endregion

        #region Otros
        public List<entUsuario> BuscarUsuario(string dato)
        {
            return _datUsuario.BuscarUsuario(dato);
        }
        public List<entUsuario> BuscarCliente(string dato)
        {
            return _datUsuario.BuscarCliente(dato);
        }
        public List<entUsuario> BuscarAdministrador(string dato)
        {
            return _datUsuario.BuscarAdministrador(dato);
        }



        public entUsuario BuscarIdCliente(int idUsuario)
        {
            return _datUsuario.BuscarIdCliente(idUsuario);
        }

        public entUsuario IniciarSesion(string dato, string contra)
        {
            entUsuario u = null;
            try
            {
                if (DateTime.Now.Hour > 24)
                {
                    throw new Exception("No puede ingresar a esta hora");
                }
                else
                {
                    contra = logRecursos.GetSHA256(contra);
                    u = _datUsuario.IniciarSesion(dato, contra);
                    if (u != null)
                    {
                        if (!u.Activo)
                        {
                            return u = null;
                            throw new Exception("Usuario ha sido dado de baja");
                        }

                    }

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);

            }
            return u;
        }
        public List<entUsuario> ListarAdministradores(string dato, string orden)
        {
            if (!string.IsNullOrEmpty(dato))
            {
                return _datUsuario.BuscarAdministrador(dato);
            }
            switch (orden)
            {
                case "asc":
                    return _datUsuario.OrdenarAdministradores(1);
                case "desc":
                    return _datUsuario.OrdenarAdministradores(0);
                default:
                    return _datUsuario.ListarAdministradores();
            }           
        }

        public bool CrearSesionUsuario(entUsuario u, out List<string> errores)
        {
            try
            {
                bool isValid = ValidationHelper.TryValidateEntityMsj(u.Correo, out errores) && ValidationHelper.TryValidateEntityMsj(u.Pass, out errores) && ValidationHelper.TryValidateEntityMsj(u.UserName, out errores);
                if (isValid)
                {
                    u.Pass = logRecursos.GetSHA256(u.Pass);
                    return _datUsuario.CrearSesionUsuario(u);
                }
            }
            catch
            {

                throw new Exception("Se produjo un error al intentar crear su cuenta");
            }
            return false;
        }
        #endregion
    }
}
