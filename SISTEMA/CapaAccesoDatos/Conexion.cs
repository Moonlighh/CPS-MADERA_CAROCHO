using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaAccesoDatos
{
    public class Conexion
    {
        private static readonly Conexion _instancia = new Conexion();

        public static Conexion Instancia
        {
            get { return _instancia; }
        }

        public SqlConnection Conectar()
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = "Data Source = themikes\\SQLExpress; Initial Catalog = BD_MADERERA_CAROCHO; Integrated Security = true";
                cn.Open();
                cn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al intentar conectarse al servidor");
            }
            return cn;
        }
    }
}
