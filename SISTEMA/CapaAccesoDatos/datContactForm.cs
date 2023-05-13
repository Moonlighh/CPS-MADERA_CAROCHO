using CapaEntidad;
using System.Data;
using System.Data.SqlClient;

namespace CapaAccesoDatos
{
    public class datContactForm
    {
        private static readonly datContactForm _instancia = new datContactForm();

        public static datContactForm Instancia
        {
            get { return _instancia; }
        }
        public bool CrearFormulario(entContactForm frm)
        {
            bool creado;
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("spCrearFormulario", cn))
                { 
                    cmd.Parameters.AddWithValue("@nombre", frm.Nombre);
                    cmd.Parameters.AddWithValue("@email", frm.Email);
                    cmd.Parameters.AddWithValue("@asunto", frm.Asunto);
                    cmd.Parameters.AddWithValue("@mensaje", frm.Mensaje);
                    cmd.Parameters.AddWithValue("@ip_remitente", frm.IpRemitente);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    creado = cmd.ExecuteNonQuery() > 0;
                }
            }
            return creado;
        }
    }
}
