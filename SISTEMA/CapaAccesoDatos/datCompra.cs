using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace CapaAccesoDatos
{
    public class datCompra: IDatCompra
    {
        //Crear
        public bool CrearCompra(entCompra comp, out int idCompra)
        {
            SqlCommand cmd = null;
            SqlTransaction tran = null;
            idCompra = -1;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cn.Open();

                // Iniciar la transacción
                tran = cn.BeginTransaction();

                cmd = new SqlCommand("spCrearCompra", cn, tran)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@total", comp.Total);
                cmd.Parameters.AddWithValue("@estado", comp.Estado);
                cmd.Parameters.AddWithValue("@idUsuario", comp.Usuario.IdUsuario);
                SqlParameter id = new SqlParameter("@id", 0);
                id.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(id);

                int i = cmd.ExecuteNonQuery();

                if (i >= 1)
                {
                    idCompra = Convert.ToInt32(cmd.Parameters["@id"].Value);
                    creado = true;
                }

                if (idCompra == -1)
                {
                    throw new ApplicationException("ID COMPRA INVALIDO");
                }

                // Si no hay errores, confirmar la transacción
                tran.Commit();
            }
            catch (Exception e)
            {
                // Si hay errores, deshacer la transacción
                tran.Rollback();
                creado = false;
                throw new ApplicationException("Error al crear la compra - AD");
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                }
            }

            return creado;

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
                    objCompra.Total = Convert.ToDecimal(dr["total"]);
                    objCompra.Estado = Convert.ToBoolean(dr["estado"]);
                    objCompra.Usuario = user;

                    lista.Add(objCompra);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("No se pudo mostrar las compras - AD");
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
                throw new ApplicationException("Error al eliminar la compra - AD");
            }
            finally { cmd.Connection.Close(); }
            return eliminado;
        }

        #region OTROS
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
                    Prod.Total = Convert.ToDecimal(dr["total"]);
                    //Prod.IdProveedor = Convert.ToInt32(dr["idProveedor"]);
                    lista.Add(Prod);
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
        #endregion Otros
    }
}
