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
using System.Data.SqlClient;

namespace CapaLogica
{
    public class logUsuario : ILogUsuario
    {
        private string errorMessage = string.Empty;
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
                bool isValid = Validation.TryValidateEntityMsj(user, out lsErrores);
                if (!isValid)
                    return false;

                user.Pass = Recursos.GetSHA256(user.Pass);
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

        #region Acceso Sistema
        public bool CrearSesionUsuario(string usuario, string correo, string password, out List<string> errores)
        {
            errores = new List<string>();

            if (ValidateUsuario(usuario) != string.Empty)
            {
                errores.Add(errorMessage);
            }

            if (ValidateCorreo(correo) != string.Empty)
            {
                errores.Add(errorMessage);
            }

            if (ValidatePassword(password) != string.Empty)
            {
                errores.Add(errorMessage);
            }

            if (errores.Count > 0)
            {
                return false;
            }

            try
            {
                entRoll r = new entRoll
                {
                    IdRoll = 2,
                };
                entUsuario u = new entUsuario
                {
                    UserName = usuario,
                    Correo = correo,
                    Pass = password,
                    Roll = r,
                };
                // Verificar que el correo sea real
                bool existe = Recursos.EnviarCorreo(u.Correo, "Crear Cuenta", "Bienvenido a Maderera Carocho, se esta procediendo a crear tu cuenta. Atentamente, Maderera Carocho");

                if (existe)
                {
                    // Encriptar clave e intentar crear el usuario
                    u.Pass = Recursos.GetSHA256(u.Pass);
                    return _datUsuario.CrearSesionUsuario(u);
                }
                else
                {
                    errores.Add("El correo ingresado no existe.");
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        private static byte intentos = 0; // Declarar una variable de instancia de tipo byte para contar los intentos
        private static DateTime? ultimoIntento = DateTime.Now; // Variable nullable DateTime para almacenar el tiempo del último intento
        public void IntentosPermitidos()
        {
            if (intentos == 3)
            {
                TimeSpan timeSpan = DateTime.Now - ultimoIntento.Value;
                int minutos = (int)timeSpan.TotalMinutes;
                int segundos = timeSpan.Seconds;
                string tiempoTranscurrido = string.Format("{0} minutos y {1} segundos", minutos, segundos);
                if (DateTime.Now - ultimoIntento.Value <= TimeSpan.FromMinutes(5))
                {
                    // Ha pasado menos de 5 minutos desde el último intento, se muestra un mensaje de error
                    throw new Exception($"Ha excedido el número máximo de intentos. Vuelva a intentarlo después de 5 minutos. Tiempo transcurrido: {tiempoTranscurrido}");
                }

                // Ha pasado más de 5 minutos desde el último intento o es la primera vez, se reinician los intentos a cero
                intentos = 0;
                ultimoIntento = DateTime.Now;
            }
        }
        public entUsuario IniciarSesion(string dato, string contra)
        {
            IntentosPermitidos();
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
                        contra = Recursos.GetSHA256(contra);
                        u = _datUsuario.IniciarSesion(dato, contra);
                        if (u != null)// El usuario existe
                        {
                            intentos = 0; // Resetear los intentos
                            if (!u.Activo)
                            {
                                u = null;
                                throw new Exception("Usted ya no puede ingresar al sistema");
                            }

                        }
                        else
                        {
                            intentos++;
                            if (intentos == 3)
                            {
                                ultimoIntento = DateTime.Now;
                                IntentosPermitidos();
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
        public bool RestablecerPassword(string password, string codigo)
        {
            bool restablecer = false;
            try
            {
                codigo = Recursos.GetSHA256(codigo);
                if (codigo == codigoGenerado)
                {
                    if (ValidatePassword(password) != string.Empty) // Verificar que se cumpla el formato
                    {
                        entUsuario user = _datUsuario.ListarUsuarios().Where(u => u.Correo == correoDestino).SingleOrDefault();
                        password = Recursos.GetSHA256(password);
                        restablecer = _datUsuario.RestablecerPassword(user.IdUsuario, password, correoDestino);
                    }
                    else
                    {
                        throw new Exception(errorMessage); // Mostrar el error capturado
                    }
                }
                else
                {
                    throw new Exception("El codigo ingresado no coincide con el autorizado");
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return restablecer;
        }
        
        private static string correoDestino = string.Empty;
        private static string codigoGenerado = string.Empty;
        public bool EnviarCodigoRestablecerPass(string correo)
        {
            bool enviado = false;
            try
            {
                // Verificar que correo tenga el formato correcto
                if (ValidateCorreo(correo) == string.Empty)
                {
                    // Verificar que exista el correo en la base de datos
                    var existe = _datUsuario.ListarUsuarios().Where(u => u.Correo == correo).SingleOrDefault();

                    if (existe != null)
                    {
                        // Enviar codigo generado al correo electronico
                        codigoGenerado = Recursos.GenerarClave();
                        string asunto = "Restablecer contraseña";
                        string mensaje = $@"
                        <!DOCTYPE html>
                        <html lang=""es"">
                            <head>
                              <meta charset=""UTF-8"">
                              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                              <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css"">
                              <style>
                                body {{
                                  background-color: #f4f4f4;
                                }}

                                .container {{
                                  max-width: 600px;
                                  margin: 0 auto;
                                  padding: 20px;
                                  background-color: #ffffff;
                                  border-radius: 5px;
                                  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                }}

                                .container h3 {{
                                  color: #333333;
                                  font-size: 24px;
                                  margin-bottom: 20px;
                                }}

                                .container p {{
                                  color: #555555;
                                  font-size: 16px;
                                  line-height: 1.5;
                                  margin-bottom: 10px;
                                }}
                              </style>
                            </head>

                            <body>
                              <div class=""container"">
                                <center><h2>Hola, {existe.UserName}</h2></center>
                                <p>Recibimos una solicitud para restablecer la contraseña de su cuenta. Para completar el proceso de
                                  restablecimiento, por favor utilice el siguiente código de seguridad:
                                  <b>{codigoGenerado}</b>
                                </p>
                                <p>Ingrese este código en el formulario de restablecimiento de contraseña en nuestro sitio web. Si usted no ha
                                  solicitado el restablecimiento de contraseña, por favor ignore este correo electrónico.</p>
                                <p>Atentamente, [Soporte Maderera Carocho]</p>
                              </div>
                            </body>
                        </html>";
                        bool enviarCorreo = Recursos.EnviarCorreo(correo, asunto, mensaje);
                        if (!enviarCorreo)
                        {
                            throw new Exception("No se pudo enviar el correo electronico a la siguiente dirección: " + correo);
                        }
                        else
                        {
                            enviado = true;
                        }

                        // Guardar el correo usado y encriptar el codigo generado
                        correoDestino = correo;
                        codigoGenerado = Recursos.GetSHA256(codigoGenerado);
                    }
                    // Dejar la cadena codigo vacia y producir la excepcion
                    else
                    {
                        throw new Exception("El correo no esta registrado en el sistema");
                    }
                }
                else
                {
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception e)
            {
                // Propagar la excepcion
                throw new Exception(e.Message);
            }
            return enviado;
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
        #endregion

        #region Validaciones
        public string ValidateUsuario(string usuario)
        {
            if (!Regex.IsMatch(usuario, @"^[a-zA-ZñÑ]{5,20}$"))
            {
                return errorMessage = "El nombre de usuario no es válido (Solo se aceptan de 5 a 20 letras).";
            }
            return string.Empty;
        }

        public string ValidateCorreo(string correo)
        {
            if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                return errorMessage = "El correo electrónico no es válido (Solo se aceptan correos de google).";
            }
            return string.Empty;

        }

        public string ValidatePassword(string password)
        {
            if (!Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%^&+=]).{8,}$"))
            {
                return errorMessage = "La contraseña no es válida (Debe contener al menos un número, una letra, un carácter especial y como mínimo 8 caracteres).";
            }
            return string.Empty;
        }
        #endregion
    }
}
