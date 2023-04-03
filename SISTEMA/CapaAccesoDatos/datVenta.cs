using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaAccesoDatos
{
    public class datVenta
    {
        private static readonly datVenta _instancia = new datVenta();
        public static datVenta Instancia
        {
            get { return _instancia; }
        }
        //Crear
        public int CrearVenta(entVenta venta)
        {
            SqlCommand cmd = null;
            int idcompra = -1;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@total", venta.Total);
                cmd.Parameters.AddWithValue("@idCliente", venta.Cliente.IdCliente);
                SqlParameter id = new SqlParameter("@idventa", 0);
                id.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(id);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i==1)
                {
                    idcompra = Convert.ToInt32(cmd.Parameters["@idventa"].Value);
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
            return idcompra;
        }
        //Leer
        public List<entVenta> ListarVenta( int id)
        {
            SqlCommand cmd = null;
            List<entVenta> lista = new List<entVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarVenta", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entVenta ven = new entVenta();
                    ven.IdVenta = Convert.ToInt32(dr["idVenta"]);
                    ven.Fecha = Convert.ToDateTime(dr["fecha"]);
                    ven.Total = Convert.ToDouble(dr["total"]);
                    ven.Estado = Convert.ToBoolean(dr["estado"]);
                    entCliente cli = new entCliente();
                    cli.IdCliente = Convert.ToInt16(dr["idCliente"]);
                    cli.RazonSocial = dr["razonSocial"].ToString();
                    ven.Cliente = cli;
                    lista.Add(ven);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            finally { 
                cmd.Connection.Close(); 
            }
            return lista;
        }                    
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
                    Prod.Total = Convert.ToDouble(dr["total"]);
                    Prod.Estado = Convert.ToBoolean(dr["estado"]);
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
                    Prod.Total = Convert.ToDouble(dr["total"]);
                    Prod.Estado = Convert.ToBoolean(dr["estado"]);
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
                MessageBox.Show(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return actualizarVenta;
        }
    }
}
