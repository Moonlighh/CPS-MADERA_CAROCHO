using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entEmpleado
    {
        private int idEmpleado;
        private string nombres;
        private string dni;
        private string telefono;
        private string direccion;
        private DateTime f_inicio;
        private DateTime f_fin;
        private double salario;
        private string descripcion;
        private bool estEmpleado;
        private entTipoEmpleado tipo;
        private entUbigeo ubigeo;

        #region Get and Set
        public int IdEmpleado { get => idEmpleado; set => idEmpleado = value; }
        public string Nombres { get => nombres; set => nombres = value; }
        public string Dni { get => dni; set => dni = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public DateTime F_inicio { get => f_inicio; set => f_inicio = value; }
        public DateTime F_fin { get => f_fin; set => f_fin = value; }
        public double Salario { get => salario; set => salario = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public bool EstEmpleado { get => estEmpleado; set => estEmpleado = value; }
        public entTipoEmpleado Tipo { get => tipo; set => tipo = value; }
        public entUbigeo Ubigeo { get => ubigeo; set => ubigeo = value; }
        #endregion
    }
}
