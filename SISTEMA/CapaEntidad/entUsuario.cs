using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entUsuario
    {
        private int idUsuario;
        private string razonSocial;
        private string dni;
        private string telefono;
        private string direccion;
        private entUbigeo ubigeo;
        private DateTime fechaCreacion;
        private string correo;
        private string userName;
        private string pass;
        private entRol rol;
        private entRoll roll;
        private bool activo;


        #region Get and Set
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }
        public string Dni
        {
            get { return dni; }
            set { dni = value; }

        }
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        public entUbigeo Ubigeo
        {
            get { return ubigeo; }
            set { ubigeo = value; }
        }

        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public string Correo { get => correo; set => correo = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Pass { get => pass; set => pass = value; }
        public entRol Rol { get => rol; set => rol = value; }
        public bool Activo { get => activo; set => activo = value; }
        public entRoll Roll { get => roll; set => roll = value; }
        #endregion Get and Set
    }
}
