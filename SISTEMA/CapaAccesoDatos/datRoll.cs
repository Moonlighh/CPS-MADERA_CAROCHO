using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

namespace CapaAccesoDatos
{
    public class datRoll
    {
        private static readonly datRoll _instancia = new datRoll();
        public static datRoll Instancia
        {
            get { return _instancia; }
        }

        public List<entRoll> ListarRoll()
        {
            SqlCommand cmd = null;
            List<entRoll> lista = new List<entRoll>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarRol", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entRoll rol = new entRoll
                    {
                        IdRoll = Convert.ToInt32(dr["idRol"]),
                        Descripcion = dr["descripcion"].ToString(),
                    };

                    lista.Add(rol);
                }
               
                    
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
    }
}
