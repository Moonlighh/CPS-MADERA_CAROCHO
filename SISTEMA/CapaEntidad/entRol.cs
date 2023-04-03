using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public enum entRol
    {
        // enum permite poner constantes listadas con nombre y valor
        Administrador = 1, Cliente = 2, Empleado = 3
      
    }
    public class entRoll
    {
        private int idRoll;
        private String descripcion;

        public int IdRoll { get => idRoll; set => idRoll = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}
