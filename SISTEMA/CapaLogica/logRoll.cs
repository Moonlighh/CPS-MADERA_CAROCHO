using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaAccesoDatos;

namespace CapaLogica
{
    public class logRoll
    {
        private static readonly logRoll _instancia = new logRoll();
        public static logRoll Instancia
        {
            get { return _instancia; }
        }

        public List<entRoll> ListarRol()
        {
            return datRoll.Instancia.ListarRoll();
        }
    }
}
