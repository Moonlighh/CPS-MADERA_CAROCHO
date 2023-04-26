using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace CapaAccesoDatos
{
    public class datCompra
    {
        private static readonly datCompra _instancia = new datCompra();
        public static datCompra Instancia
        {
            get { return _instancia; }
        }

        //Crear
        public int CrearCompra(entCompra comp)
        {
            SqlCommand cmd = null;
            int idCompra = -1;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearCompra", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@total", comp.Total);
                cmd.Parameters.AddWithValue("@estado", comp.Estado);
                cmd.Parameters.AddWithValue("@idUsuario", comp.Usuario.IdUsuario);
                SqlParameter id = new SqlParameter("@id", 0);
                id.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(id);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i >= 1)
                {
                    idCompra = Convert.ToInt32(cmd.Parameters["@id"].Value);
                }
                if (idCompra == -1)
                {
                    MessageBox.Show("Id INVALIDO");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error al insertar una compra procedimiento spCrearCompra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return idCompra;
        }
        //Leer
        public List<entCompra> ListarCompra()
        {
            List<entCompra> lista = new List<entCompra>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario user = new entUsuario()
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                        RazonSocial = dr["comprador"].ToString()
                    };
                    entCompra objCompra = new entCompra();
                    objCompra.IdCompra = Convert.ToInt32(dr["idCompra"]);
                    objCompra.Fecha = Convert.ToDateTime(dr["fecha"]);
                    objCompra.Total = Convert.ToDouble(dr["total"]);
                    objCompra.Estado = Convert.ToBoolean(dr["estado"]);
                    objCompra.Usuario = user;

                    lista.Add(objCompra);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "spCrearCompra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        //Eliminar - Deshabilitar
        public bool EliminarCompra(int idcompra)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCompra", idcompra);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    eliminado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return eliminado;
        }

        #region OTROS
        public int GenerarID(string tipo)
        {
            SqlCommand cmd = null;
            int id = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();

                cmd = new SqlCommand("spReturnID", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt16(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return id;

        }
        public List<entCompra> BuscarCompra(string busqueda)
        {
            List<entCompra> lista = new List<entCompra>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarCompra", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entCompra Prod = new entCompra();
                    Prod.IdCompra = Convert.ToInt32(dr["idCompra"]);
                    Prod.Fecha = Convert.ToDateTime(dr["fecha"]);
                    Prod.Total = Convert.ToDouble(dr["total"]);
                    //Prod.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    lista.Add(Prod);
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
            return lista;
        }
        #endregion Otros
    }
}
