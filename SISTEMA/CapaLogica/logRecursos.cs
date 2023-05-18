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
    public class logRecursos
    {
        //Generamos una clave automatica que enviaremos al usuario - no se vuelve a repetir
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);//Retorna un codigo unico-solo caracteres alfanumericos-longitud de la clave
            return clave;
        }
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
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
            }
            catch
            {
                return false;
                throw new Exception("No se pudo enviar su codigo de restablecimiento a su correo " + correo + " intentelo de nuevo o mas tarde");        
            }

            return true;
        }
        public void SendResetPasswordEmail(string recipientEmail, string confirmationLink)
        {
            string senderEmail = "example@gmail.com";
            string senderPassword = "password";

            // Configurar el cliente SMTP
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            // Crear el correo electrónico
            var mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = "Restablecer contraseña",
                Body = $"Haga clic en el siguiente enlace para restablecer su contraseña: {confirmationLink}"
            };

            // Agregar el botón de confirmación al cuerpo del mensaje
            mailMessage.IsBodyHtml = true;
            mailMessage.Body += "<br/><br/><a href='" + confirmationLink + "'><button>Confirmar</button></a>";

            // Enviar el correo electrónico
            smtpClient.Send(mailMessage);
        }
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
