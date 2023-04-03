using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
namespace CapaAccesoDatos
{
    public class datEmpleado
    {
        private static readonly datEmpleado _instancia = new datEmpleado();

        public static datEmpleado Instancia
        {
            get { return _instancia; }
        }
        #region CRUD
        //Crear
        public bool CrearEmpleado(entEmpleado emp)
        {
            SqlCommand cmd = null;
            bool creado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spCrearEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombres", emp.Nombres);
                cmd.Parameters.AddWithValue("@dni", emp.Dni);
                cmd.Parameters.AddWithValue("@telefono", emp.Telefono);
                cmd.Parameters.AddWithValue("@direccion", emp.Direccion);
                cmd.Parameters.AddWithValue("@salario", emp.Salario);
                cmd.Parameters.AddWithValue("@descripcion", emp.Descripcion);
                cmd.Parameters.AddWithValue("@idTipo_Empleado", emp.Tipo.IdTipo_Empleado);
                cmd.Parameters.AddWithValue("@idUbigeo", emp.Ubigeo.IdUbigeo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    creado = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR AL INSERTAR EMPLEADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return creado;

        }
        //Leer
        public List<entEmpleado> ListarEmpleado()
        {
            SqlCommand cmd = null;
            List<entEmpleado> lista = new List<entEmpleado>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUbigeo ubi = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString()
                    };
                    entTipoEmpleado tipo = new entTipoEmpleado
                    {
                        Nombre = dr["tipo"].ToString()
                    };
                    entEmpleado emp = new entEmpleado
                    {
                        IdEmpleado = Convert.ToInt32(dr["idEmpleado"]),
                        Nombres = dr["nombres"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        Salario = Convert.ToDouble(dr["salario"]),
                        Descripcion = dr["descripcion"].ToString(),
                        Tipo = tipo,
                        Ubigeo = ubi
                    };
                    lista.Add(emp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "EROR AL MOSTRAR EMPLEADOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
        //Actualizar

        public bool ActualizarEmpleado(entEmpleado emp)
        {
            SqlCommand cmd = null;
            bool actualiza = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spActualizarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", emp.IdEmpleado);
                cmd.Parameters.AddWithValue("@nombres", emp.Nombres);
                cmd.Parameters.AddWithValue("@dni", emp.Dni);
                cmd.Parameters.AddWithValue("@telefono", emp.Telefono);
                cmd.Parameters.AddWithValue("@direccion", emp.Direccion);
                cmd.Parameters.AddWithValue("@f_inicio", emp.F_inicio);
                cmd.Parameters.AddWithValue("@f_fin", emp.F_fin);
                cmd.Parameters.AddWithValue("@salario", emp.Salario);
                cmd.Parameters.AddWithValue("@descripcion", emp.Descripcion);
                cmd.Parameters.AddWithValue("@estEmpleado", emp.EstEmpleado);
                cmd.Parameters.AddWithValue("@idTipo_Empleado", emp.Tipo.IdTipo_Empleado);
                cmd.Parameters.AddWithValue("@idUbigeo", emp.Ubigeo.IdUbigeo);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    actualiza = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { cmd.Connection.Close(); }
            return actualiza;
        }

        //Eliminar - Deshabilitar
        public bool EliminarEmpleado(int id)
        {
            SqlCommand cmd = null;
            bool eliminado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", id);
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
        #endregion CRUD

        public List<entEmpleado> BuscarEmpleado(string busqueda)
        {
            List<entEmpleado> lista = new List<entEmpleado>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entUbigeo ubi = new entUbigeo
                    {
                        Distrito = dr["distrito"].ToString()
                    };
                    entTipoEmpleado tipo = new entTipoEmpleado
                    {
                        Nombre = dr["tipo"].ToString()
                    };
                    entEmpleado emp = new entEmpleado
                    {
                        IdEmpleado = Convert.ToInt32(dr["idEmpleado"]),
                        Nombres = dr["nombres"].ToString(),
                        Dni = dr["dni"].ToString(),
                        Telefono = dr["telefono"].ToString(),
                        Direccion = dr["direccion"].ToString(),
                        Salario = Convert.ToDouble(dr["salario"]),
                        Descripcion = dr["descripcion"].ToString(),
                        Tipo = tipo,
                        Ubigeo = ubi
                    };
                    lista.Add(emp);
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
        #region OTROS
        public entEmpleado BuscarIdEmpleado(int idemp)
        {
            SqlCommand cmd = null;
            entEmpleado emp = new entEmpleado();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarIdEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idEmpleado", idemp);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    emp.IdEmpleado = Convert.ToInt32(dr["idEmpleado"]);
                    emp.Nombres = dr["nombres"].ToString();
                    emp.Dni = dr["dni"].ToString();
                    emp.Telefono = dr["telefono"].ToString();
                    emp.Direccion = dr["direccion"].ToString();
                    emp.F_inicio = Convert.ToDateTime(dr["f_inicio"]);
                    emp.F_fin = Convert.ToDateTime(dr["f_inicio"]);
                    emp.Salario = Convert.ToDouble(dr["salario"]);
                    emp.Descripcion = dr["descripcion"].ToString();
                    emp.EstEmpleado = Convert.ToBoolean(dr["estEmpleado"]);
                    entTipoEmpleado tipo = new entTipoEmpleado();

                    tipo.Nombre = dr["tipo"].ToString();
                    emp.Tipo = tipo;
                    entUbigeo ubi = new entUbigeo();
                    ubi.Distrito = dr["distrito"].ToString();
                    emp.Ubigeo = ubi;
                }
            }
            catch (Exception e)
            { throw e; }
            finally { cmd.Connection.Close(); }
            return emp;
        }
        #endregion OTROS
    }
}