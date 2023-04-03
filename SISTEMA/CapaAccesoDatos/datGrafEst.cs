using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaAccesoDatos
{
    public class datGrafEst
    {
        private static readonly datGrafEst _instance = new datGrafEst();
        public static datGrafEst Instancia
        {
            get { return _instance; }
        }

        public ArrayList ProductosPreferidos(object chart)
        {
            SqlCommand cmd = null;
            ArrayList NombreMadera = new ArrayList();
            ArrayList CantMadera = new ArrayList();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spProductosPreferidos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    NombreMadera.Add(dr.GetString(0));
                    CantMadera.Add(dr.GetInt16(1));
                }
                
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return NombreMadera;
        }
    }
}
