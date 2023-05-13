using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logContactForm
    {
        private static readonly logContactForm _instance = new logContactForm();
        public static logContactForm Instance
        {
            get { return _instance; }
        }

        public bool CrearFormulario(entContactForm frm, out List<string> errores)
        {
            if (!ValidationHelper.TryValidateEntityMsj(frm, out errores))
            {
                return false;
            }
            return datContactForm.Instancia.CrearFormulario(frm);
        }
    }
}
