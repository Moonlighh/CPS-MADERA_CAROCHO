using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.IO;
using System.Security.Cryptography;

namespace CapaLogica
{
    public class Recursos
    {
        // Generamos una clave automatica que enviaremos al usuario - no se vuelve a repetir
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);//Retorna un codigo unico-solo caracteres alfanumericos-longitud de la clave
            return clave;
        }
        
        // Enviar correo para cualquier metodo
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool enviado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);//Para quien
                mail.From = new MailAddress("rawrdaarling@gmail.com");//De quien
                mail.Subject = asunto;//Asunto
                mail.Body = mensaje;//Mensaje del cuerpo
                mail.IsBodyHtml = true;//Enviamos el correo en formato html

                var smtp = new SmtpClient()//Se encargara de hacer la operacion para enviar el correo
                {
                    Credentials = new NetworkCredential("rawrdaarling@gmail.com", "zuhlnwsmrckrzbjz"),//No es la contraseña de tu correo sino una que te genera google
                    Host = "smtp.gmail.com", //Server que usa gmail para envair los correos
                    Port = 587,//Puerto que usa para enviar los correos
                    EnableSsl = true//Habilitamos el certificado de seguridad
                };

                smtp.Send(mail);//Enviamos el correo
                enviado = true;
            }
            catch
            {
                throw new Exception("El correo no existe o la conexión fue rechazada");        
            }

            return enviado;
        }

        // Encriptar contenido
        /// <summary>
        /// Obtiene el hash SHA-256 de una cadena de texto.
        /// </summary>
        /// <param name="str">La cadena de texto a hashear.</param>
        /// <returns>El hash SHA-256 en formato hexadecimal.</returns>
        public static string GetSHA256(string str)
        {
            // Crea una instancia del algoritmo de hash SHA-256
            SHA256 sha256 = SHA256Managed.Create();

            // Crea una instancia de la codificación ASCII para convertir la cadena en bytes
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] stream = null; // Variable para almacenar el resultado del hash
            StringBuilder sb = new StringBuilder(); // StringBuilder para construir la representación hexadecimal del hash

            // Calcula el hash SHA-256 de la cadena de texto
            stream = sha256.ComputeHash(encoding.GetBytes(str));

            // Convierte los bytes del hash en una representación hexadecimal y los agrega al StringBuilder
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            // Devuelve el hash SHA-256 en formato hexadecimal
            return sb.ToString();
        }
    }
}
