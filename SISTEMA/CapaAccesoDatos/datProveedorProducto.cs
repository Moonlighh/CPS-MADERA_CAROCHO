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
    public class datProveedorProducto
    {
        private static datProveedorProducto _instancia = new datProveedorProducto();

        public static datProveedorProducto Instancia
        {
            get { return _instancia; }
        }

        public List<entProveedorProducto> ListarProveedorProducto()
        {
            SqlCommand cmd = null;
            List<entProveedorProducto> lista = new List<entProveedorProducto>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarProveedorProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor p = new entProveedor
                    {
                        IdProveedor = Convert.ToInt32(dr["idProveedor"]),
                        RazonSocial = dr["proveedor"].ToString(),
                        EstProveedor = Convert.ToBoolean(dr["estProveedor"])
                    };
                    entProducto prod = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["madera"].ToString(),
                        Longitud = Convert.ToInt32(dr["longitud"]),
                        Diametro = Convert.ToInt32(dr["diametro"]),
                        Stock = Convert.ToInt32(dr["stock"]),
                        PrecioVenta = Convert.ToDouble(dr["precioVenta"])
                    };
                    entProveedorProducto obj = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                        Proveedor = p,
                        Producto = prod,
                        PrecioCompra = Convert.ToDouble(dr["precio"])
                    };
                    lista.Add(obj);
                }
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo listar los productos");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
        
        public List<entProveedorProducto> ListarProductoAdmin()
        {
            SqlCommand cmd = null;
            List<entProveedorProducto> lista = new List<entProveedorProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarProductoAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProveedor provedor = new entProveedor
                    {
                        RazonSocial = dr["razonSocial"].ToString(),
                    };
                    entTipoProducto tipo = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString()
                    };
                    entProducto producto = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        PrecioVenta = Convert.ToDouble(dr["precioVenta"]),
                        Stock = Convert.ToInt32(dr["stock"]),
                        Tipo = tipo
                    };
                    entProveedorProducto prov = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                        PrecioCompra = Convert.ToDouble(dr["precioCompra"]),
                        Producto = producto,
                        Proveedor = provedor
                    };

                    lista.Add(prov);
                }

            }
            catch (Exception e)
            {
                throw new Exception("No se pudo listar los productos");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public List<entProveedorProducto> ListarProductoCliente()
        {
            SqlCommand cmd = null;
            List<entProveedorProducto> lista = new List<entProveedorProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarProductoCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    
                    entTipoProducto tipo = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString()
                    };
                    entProducto producto = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        PrecioVenta = Convert.ToDouble(dr["precioVenta"]),
                        Stock = Convert.ToInt32(dr["stock"]),
                        Tipo = tipo
                    };
                    entProveedorProducto prov = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                        Producto = producto
                    };
                    lista.Add(prov);
                    
                }

            }
            catch (Exception e)
            {
                throw new Exception("No se pudo listar los productos" + e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public List<entProveedorProducto> BuscarProductoAdmin(string busqueda)
        {
            List<entProveedorProducto> lista = new List<entProveedorProducto>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarProductoAdmin", cn);
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipoProducto tipo = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString()
                    };
                    entProducto producto = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        PrecioVenta = Convert.ToDouble(dr["precioVenta"]),
                        Stock = Convert.ToInt32(dr["stock"]),
                        Tipo = tipo
                    };
                    entProveedorProducto prov = new entProveedorProducto
                    {
                        PrecioCompra = Convert.ToDouble(dr["precioCompra"]),
                        Producto = producto
                    };

                    lista.Add(prov);
                }
            }
            catch (Exception e)
            {

                throw new Exception("No se pudo listar los productos");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public List<entProveedorProducto> BuscarProductoCliente(string busqueda)
        {
            List<entProveedorProducto> lista = new List<entProveedorProducto>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spBuscarProductoCliente", cn);
                cmd.Parameters.AddWithValue("@Campo", busqueda);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipoProducto tipo = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString()
                    };
                    entProducto producto = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        PrecioVenta = Convert.ToDouble(dr["precioVenta"]),
                        Stock = Convert.ToInt32(dr["stock"]),
                        Tipo = tipo
                    };
                }
            }
            catch (Exception e)
            {

                throw new Exception("No se pudo listar los productos");
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
    }
}
