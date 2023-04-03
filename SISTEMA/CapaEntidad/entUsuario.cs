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
        private entCliente cliente;
        private DateTime fechaCreacion;
        private String userName;
        private String correo;
        private String pass;
        private entRol rol;
        private entRoll roll;
        private Boolean activo;
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        public entCliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }
        public entRol Rol
        {
            get { return rol; }
            set { rol = value; }
        }
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public entRoll Roll { get => roll; set => roll = value; }
    }
}
