using CapaAccesoDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logEmpleado
    {
        private static readonly logEmpleado _instancia = new logEmpleado();

        public static logEmpleado Instancia
        {
            get { return _instancia; }
        }

        #region CRUD
        public bool CrearEmpleado(entEmpleado pro)
        {
            return datEmpleado.Instancia.CrearEmpleado(pro);
        }
        public List<entEmpleado> ListarEmpleado(string busqueda, string orden)
        {
            switch (orden)
            {
                case "asc": return datEmpleado.Instancia.Ordenar(1);
                case "desc": return datEmpleado.Instancia.Ordenar(0);
                default:
                    break;
            }
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                return datEmpleado.Instancia.ListarEmpleado();
            }
            else
            {
                return datEmpleado.Instancia.BuscarEmpleado(busqueda);
            }
        }
        public bool ActualizarEmpleado(entEmpleado pro)
        {
            return datEmpleado.Instancia.ActualizarEmpleado(pro);
        }
        public bool DeshabilitarEmpleado(int id)
        {
            return datEmpleado.Instancia.DeshabilitarEmpleado(id);
        }
        #endregion CRUD

        public entEmpleado BuscarIdEmpleado(int idEmpleado)
        {
            return datEmpleado.Instancia.BuscarIdEmpleado(idEmpleado);
        }
    }
}
