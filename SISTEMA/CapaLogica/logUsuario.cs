using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;//Validaciones
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Windows.Forms;

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
            try
            {
                bool isValid = ValidationHelper.TryValidateEntityMsj(user, out lsErrores);
                if (!isValid)
                    return false;
                
                user.Pass = logRecursos.GetSHA256(user.Pass);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
                if (!(string.IsNullOrEmpty(dato) || string.IsNullOrEmpty(contra)))
                {
                    if (DateTime.Now.Hour > 24)
                    {
                        throw new Exception("No se puede ingresar despues de las 22:00 horas");
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
                                throw new Exception("Usted ya no puede ingresar al sistema");
                            }

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
            bool creado = false;
            errores = new List<string>();

            try
            {
                if (u == null)
                {
                    errores.Add("El usuario es nulo");
                    return false;
                }
                bool isValid = ValidationHelper.TryValidateEntityMsj(u.Correo, out errores) &&
                               ValidationHelper.TryValidateEntityMsj(u.Pass, out errores) &&
                               ValidationHelper.TryValidateEntityMsj(u.UserName, out errores);
                if (isValid)
                {
                    u.Pass = logRecursos.GetSHA256(u.Pass);
                    creado = _datUsuario.CrearSesionUsuario(u);
                }
            }
            catch
            {
                throw new Exception("Se produjo un error al intentar crear su cuenta");
            }

            return creado;
        }
        
        public string EnviarCodigoRestablecerPass(string correo)
        {
            string codigo = string.Empty;
            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(correo))
                {
                    var existe = _datUsuario.ListarUsuarios().Where(u => u.Correo == correo).SingleOrDefault();
                    if (existe != null)
                    {
                        codigo = logRecursos.GenerarClave();
                        string asunto = "Restablecer contraseña";
                        string mensaje = "Hola,\r\n\r\nRecibimos una solicitud para restablecer la contraseña de su cuenta." +
                            " Para completar el proceso de restablecimiento, por favor utilice el siguiente código de seguridad:" +
                            "\r\n\r\n"+codigo+ "\r\n\r\n. Ingrese este código en el formulario de restablecimiento de " +
                            "contraseña en nuestro sitio web. Si usted no ha solicitado el restablecimiento de contraseña, por favor " +
                            "ignore este correo electrónico.\nAtentamente, [Suport Maderera Carocho]";
                        
                       bool enviarCorreo = logRecursos.EnviarCorreo(correo, asunto, mensaje);
                       if (!enviarCorreo) {
                            throw new Exception("No se pudo enviar el correo electronico a la siguiente dirección: " + correo);
                       }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return codigo;
        }

        public bool RestablecerPassword(string correo, string password, string codigo)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
