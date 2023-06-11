using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaAccesoDatos
{
    public class datVenta:IDatVenta
    {
        //Crear
        public bool CrearVenta(entVenta venta, out int idVenta)
        {
            SqlCommand cmd = null;
            SqlTransaction tran = null;
            idVenta = -1;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cn.Open();

                // Iniciar la transacción
                tran = cn.BeginTransaction();

                cmd = new SqlCommand("spCrearVenta", cn, tran)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@total", venta.Total);
                cmd.Parameters.AddWithValue("@estado", venta.Estado);
                cmd.Parameters.AddWithValue("@idUsuario", venta.Usuario.IdUsuario);
                SqlParameter id = new SqlParameter("@id", 0);
                id.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(id);

                int i = cmd.ExecuteNonQuery();

                if (i >= 1)
                {
                    idVenta = Convert.ToInt32(cmd.Parameters["@id"].Value);
                    creado = true;
                }

                if (idVenta == -1)
                {
                    throw new ApplicationException("ID VENTA INVALIDO");
                }

                // Si no hay errores, confirmar la transacción
                tran.Commit();
            }
            catch (Exception e)
            {
                // Si hay errores, deshacer la transacción
                tran.Rollback();
                creado = false;
                throw new ApplicationException("Error al crear la venta - AD");
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
        public List<entVenta> ListarVenta(int idCliente)
        {
            List<entVenta> lista = new List<entVenta>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarVenta", cn);
                cmd.Parameters.AddWithValue("idCliente", idCliente);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUsuario user = new entUsuario()
                    {
                        IdUsuario = Convert.ToInt32(dr["idUsuario"]),
                    };
                    entVenta objVenta = new entVenta();
                    objVenta.IdVenta = Convert.ToInt32(dr["idVenta"]);
                    objVenta.Fecha = Convert.ToDateTime(dr["fecha"]);
                    objVenta.Total = Convert.ToDecimal(dr["total"]);
                    objVenta.Estado = Convert.ToBoolean(dr["estado"]);
                    objVenta.Usuario = user;

                    lista.Add(objVenta);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }


        #region OTROS
        public List<entVenta> BuscarVenta(string busqueda)
        {
            List<entVenta> lista = new List<entVenta>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entVenta Prod = new entVenta();
                    Prod.IdVenta = Convert.ToInt32(dr["idVenta"]);
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






        //Leer ventas Pagadas
        public List<entVenta> ListarVentaPagada(DateTime fecha)
        {
            SqlCommand cmd = null;
            List<entVenta> lista = new List<entVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarVentaPagada", cn);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entVenta Prod = new entVenta();
                    Prod.IdVenta = Convert.ToInt32(dr["idVenta"]);
                    Prod.Fecha = Convert.ToDateTime(dr["fecha"]);
                    Prod.Total = Convert.ToDecimal(dr["total"]);
                    Prod.Estado = Convert.ToBoolean(dr["estado"]);
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
        //Listar venta no pagada 
        public List<entVenta> ListarVentaNoPagada(DateTime fecha)
        {
            SqlCommand cmd = null;
            List<entVenta> lista = new List<entVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarVentaNoPagada", cn);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entVenta Prod = new entVenta();
                    Prod.IdVenta = Convert.ToInt32(dr["idVenta"]);
                    Prod.Fecha = Convert.ToDateTime(dr["fecha"]);
                    Prod.Total = Convert.ToDecimal(dr["total"]);
                    Prod.Estado = Convert.ToBoolean(dr["estado"]);
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
        //Actualizar
        public bool ActualizarVenta(int idVenta, bool estado)
        {
            SqlCommand cmd = null;
            bool actualizarVenta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                cmd.Parameters.AddWithValue("@estado", estado);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualizarVenta = true;
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
            return actualizarVenta;
        }
    }
}
