﻿using System;
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
            bool resultado = false;
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
                resultado = true;//Caso en el que el correo se envio exitosamente
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
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
