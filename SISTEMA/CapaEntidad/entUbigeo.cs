using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entUbigeo
    {
        string  idUbigeo;
        string departamento;
        string provincia;
        string distrito;

        public string IdUbigeo { get => idUbigeo; set => idUbigeo = value; }
        public string Departamento { get => departamento; set => departamento = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string Distrito { get => distrito; set => distrito = value; }
    }
}
