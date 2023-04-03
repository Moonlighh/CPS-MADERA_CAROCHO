using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class entAdministrador
    {
        private int idAdministrador;
        private string usuario;
        private string contra;

        #region Constructores
        public entAdministrador()
        {

        }

        public entAdministrador(int idAdministrador, string usuario, string contra)
        {
            this.idAdministrador = idAdministrador;
            this.usuario = usuario;
            this.contra = contra;
        }
        #endregion Constructores

        #region Get and Set
        public int IdAdministrador
        {
            get { return idAdministrador; }
            set { idAdministrador = value; }
        }

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public string Contra
        {
            get { return contra; }
            set { contra = value; }
        }
        #endregion Get and Set
    }

}
