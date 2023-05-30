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
        public bool CrearDetCompra(entDetCompra comp, int idCompra)
        {
            return datDetCompra.Instancia.CrearDetCompra(comp, idCompra);
        }
        public List<entDetCompra> MostrarDetalleCompra(int idUsuario, int idCompra)
        {
            try
            {
                if (idCompra <= 0 || idUsuario <= 0)
                {
                    return new List<entDetCompra>();
                }
                return datDetCompra.Instancia.MostrarDetalleCompra(idUsuario, idCompra);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
