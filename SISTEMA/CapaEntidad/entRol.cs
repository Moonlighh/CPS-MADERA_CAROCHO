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
        private int idrol;
        private String descripcion;

        public int Idrol { get => idrol; set => idrol = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}
