using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logDetCompra
    {
        private static readonly logDetCompra _instancia = new logDetCompra();

        public static logDetCompra Instancia
        {
            get { return _instancia; }
        }

        #region CR
        public bool CrearDetCompra(entDetCompra comp)
        {
            return datDetCompra.Instancia.CrearDetCompra(comp);
        }
        public List<entDetCompra> MostrarDetalleCompra(int idCompra)
        {
            return datDetCompra.Instancia.MostrarDetalleCompra(idCompra);
        }
        #endregion

    }
}
