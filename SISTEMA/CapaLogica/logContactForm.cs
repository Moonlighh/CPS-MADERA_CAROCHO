using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CapaLogica
{
    public class logContactForm
    {
        private static readonly logContactForm _instance = new logContactForm();
        public static logContactForm Instance
        {
            get { return _instance; }
        }

        private static DateTime? ultimoIntento = DateTime.Now;
        private static int cantMensajes = 0;
        public bool CrearFormulario(entContactForm frm, out List<string> errores)
        {
            errores = new List<string>();
            if (cantMensajes == 5)
            {
                TimeSpan timeSpan = DateTime.Now - ultimoIntento.Value;
                int minutos = (int)timeSpan.TotalMinutes;
                int segundos = timeSpan.Seconds;
                string tiempoTranscurrido = string.Format("{0} minutos y {1} segundos", minutos, segundos);

                if (DateTime.Now - ultimoIntento.Value <= TimeSpan.FromMinutes(25))
                {
                    // Ha pasado menos de 5 minutos desde el último intento, se muestra un mensaje de error
                    errores.Add($"Ha excedido el número máximo de mensajes. Vuelva a intentarlo después de 25 minutos. Tiempo transcurrido: {tiempoTranscurrido}");
                    return false;
                }

                // Ha pasado más de 5 minutos desde el último intento o es la primera vez, se reinician los intentos a cero
                cantMensajes = 0;
                ultimoIntento = DateTime.Now;
            }
            if (cantMensajes < 5)
            {
                if (!ValidationHelper.TryValidateEntityMsj(frm, out errores))
                {
                    return false;
                }
                bool isValid = logRecursos.EnviarCorreo(frm.Email, "", "");
                if (!isValid)
                {
                    errores.Add($"{frm.Email} no es un correo valido");
                    return false;
                }
                bool creado = datContactForm.Instancia.CrearFormulario(frm);
                if (creado)
                {
                    cantMensajes++;
                    return true;
                } 
            }
            return false;
        }
    }
}
