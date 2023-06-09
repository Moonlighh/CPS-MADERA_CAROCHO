using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaAccesoDatos;
namespace CapaLogica
{
    public class logDetVenta
    {
        private static readonly logDetVenta _instancia = new logDetVenta();

        public static logDetVenta Instancia
        {
            get { return _instancia; }
        }

        #region CR
        public bool CrearDetVenta(entDetVenta comp, int idVenta)
        {
            return datDetVenta.Instancia.CrearDetVenta(comp, idVenta);
        }
        public List<entDetVenta> MostrarDetalleVenta(int idUsuario, int idVenta)
        {
            try
            {
                if (idVenta <= 0 || idUsuario <= 0)
                {
                    return new List<entDetVenta>();
                }
                return datDetVenta.Instancia.MostrarDetalleVenta(idUsuario, idVenta);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
