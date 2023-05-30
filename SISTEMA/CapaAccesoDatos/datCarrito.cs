using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace CapaAccesoDatos
{
    public class datCarrito
    {
        List<entDetCompra> detCompra = new List<entDetCompra>();//Sirve para guardar temporalmente productos al carrito

        private static readonly datCarrito _instancia = new datCarrito();
        public static datCarrito Instancia
        {
            get { return _instancia; }
        }

        #region Carrito de Compra
        public bool AgregarProductoCarrito(entCarrito car)
        {
            // Este método registra un producto en el carrito de compras de un cliente
            // Parámetros:
            // - car: objeto de la clase Carrito con la información del carrito de compras
            // Retorna:
            // - bool: true si el registro fue exitoso, false si hubo algún error

            bool creado = false; // variable para indicar si el registro fue exitoso o no

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); // instancia de la conexión a la base de datos
                using (SqlCommand cmd = new SqlCommand("spAgregarProductoCarrito", cn))
                {
                    cmd.Parameters.AddWithValue("@idCliente", car.Cliente.IdUsuario); // asignación de parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@idProveedor_Producto", car.ProveedorProducto.IdProveedorProducto);
                    cmd.Parameters.AddWithValue("@cantidad", car.Cantidad);
                    cmd.Parameters.AddWithValue("@subTotal", car.Subtotal);
                    cmd.CommandType = CommandType.StoredProcedure; // indicación del tipo de comando
                    cn.Open(); // apertura de la conexión
                    int i = cmd.ExecuteNonQuery(); // ejecución del procedimiento almacenado
                    if (i != 0) // si se registró al menos un registro, se marca como creado
                    {
                        creado = true;
                    }
                }
            }
            catch (Exception e) // si se produce una excepción, se muestra un mensaje de error
            {
                //return creado = false;
                //throw new Exception(e.Message);
            }

            return creado; // se retorna el valor de la variable creado
        }

        // Asegurarse de que siempre que se pida mostrar el carrito hay que recibir el id del usuario
        // para que un usuario no pueda modificar datos de otros usuarios
        public List<entCarrito> MostrarCarrito(int idUsuario)
        {
            // Esta función muestra los productos que un cliente tiene en su carrito de compras

            SqlCommand cmd = null;
            List<entCarrito> list = new List<entCarrito>();
            try
            {
                // Conectar a la base de datos
                SqlConnection cn = Conexion.Instancia.Conectar();
                // Crear el comando para ejecutar el procedimiento almacenado
                cmd = new SqlCommand("spMostrarCarrito", cn);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                // Ejecutar el comando y leer los datos obtenidos
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // Crear los objetos necesarios para almacenar los datos obtenidos
                    entTipoProducto tipoProducto = new entTipoProducto
                    {
                        Tipo = dr["tipo"].ToString(),
                    };
                    entProveedor proveedor = new entProveedor
                    {
                        RazonSocial = dr["razonSocial"].ToString()
                    };
                    entProducto p = new entProducto
                    {
                        IdProducto = Convert.ToInt32(dr["idProducto"]),
                        Nombre = dr["nombre"].ToString().ToUpper(),
                        Longitud = Convert.ToDouble(dr["longitud"]),
                        Diametro = Convert.ToDouble(dr["diametro"]),
                        Tipo = tipoProducto
                    };
                    entProveedorProducto pro = new entProveedorProducto
                    {
                        IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                        PrecioCompra = Convert.ToDouble(dr["precioCompra"]),
                        Producto = p,
                        Proveedor = proveedor
                    };
                    entCarrito c = new entCarrito
                    {
                        IdCarrito = Convert.ToInt32(dr["idCarrito"]),
                        Cantidad = Convert.ToInt32(dr["cantidad"]),
                        Subtotal = Convert.ToDecimal(dr["subtotal"]),
                        ProveedorProducto = pro
                    };
                    // Agregar el carrito a la lista de carritos
                    list.Add(c);
                };
            }
            catch (Exception e)
            {
                // En caso de error, mostrar un mensaje con la excepción lanzada
                throw new Exception(e.Message);
            }
            finally
            {
                // Cerrar la conexión a la base de datos
                cmd.Connection.Close();
            }
            // Devolver la lista de carritos obtenida
            return list;
        }

        public bool EditarProductoCarrito(entCarrito car)
        {
            bool editar = false;
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                using (SqlCommand cmd = new SqlCommand("spEditarProductoCarrito", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCarrito", car.IdCarrito);
                    cmd.Parameters.AddWithValue("@cantidad", car.Cantidad);
                    cmd.Parameters.AddWithValue("@subtotal", car.Subtotal);
                    try
                    {
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            editar = true;
                        else
                            editar = false;
                    }
                    catch (Exception e)
                    {
                        throw new Exception ("No se pudo editar el producto del carrito");
                    }
                }
            }
            return editar;
        }
        
        public bool EliminarProductoCarrito(int idProveedor_Producto, int idCliente)
        {
            // Esta función elimina un producto del carrito de compras

            // Inicializamos las variables
            SqlCommand cmd = null;
            bool eliminar = false;

            try
            {
                // Conectamos a la base de datos y preparamos el comando
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spEliminarProductosCarrito", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                // Añadimos los parámetros necesarios para el procedimiento almacenado
                cmd.Parameters.AddWithValue("@idProveedor_Producto", idProveedor_Producto);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);

                // Abrimos la conexión y ejecutamos el comando
                cn.Open();
                int i = cmd.ExecuteNonQuery();

                // Si se elimina algún producto, actualizamos la variable eliminar a true
                if (i != 0)
                {
                    eliminar = true;
                }
            }
            catch (Exception e)
            {
                // Si se produce una excepción, mostramos un mensaje de error
                throw new Exception(e.Message);
            }
            finally
            {
                // Cerramos la conexión
                cmd.Connection.Close();
            }

            // Devolvemos el valor de la variable eliminar
            return eliminar;
        }
        
        public List<entCarrito> Ordenar(int idUsuario, int orden)
        {
            try
            {
                using (var cn = Conexion.Instancia.Conectar())
                {
                    using (var cmd = new SqlCommand("spOrdenarCarrito", cn))
                    {
                        cmd.Parameters.AddWithValue("@orden", orden);
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.CommandType= CommandType.StoredProcedure;
                        cn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        List<entCarrito> lista = new List<entCarrito>();
                        while (dr.Read())
                        {
                            // Crear los objetos necesarios para almacenar los datos obtenidos
                            entTipoProducto tipoProducto = new entTipoProducto
                            {
                                Tipo = dr["tipo"].ToString(),                            
                            };
                            entProveedor proveedor = new entProveedor
                            {
                                RazonSocial = dr["razonSocial"].ToString()
                            };
                            entProducto p = new entProducto
                            {
                                //IdProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["nombre"].ToString().ToUpper(),
                                Longitud = Convert.ToDouble(dr["longitud"]),
                                Diametro = Convert.ToDouble(dr["diametro"]),
                                Tipo = tipoProducto
                            };
                            entProveedorProducto pro = new entProveedorProducto
                            {
                                //IdProveedorProducto = Convert.ToInt32(dr["idProveedor_Producto"]),
                                PrecioCompra = Convert.ToDouble(dr["precioCompra"]),
                                Producto = p,
                                Proveedor = proveedor
                            };
                            entCarrito c = new entCarrito
                            {
                                IdCarrito = Convert.ToInt32(dr["idCarrito"]),
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                Subtotal = Convert.ToDecimal(dr["subtotal"]),
                                ProveedorProducto = pro
                            };
                            // Agregar el carrito a la lista de carritos
                            lista.Add(c);
                        }
                        return lista;
                    }
                }
            }
            catch (Exception)
            {
                // Así no se pueda listar hay que volver a listar
                return MostrarCarrito(idUsuario);
            }
        }
        
        #endregion CarritoCompra
    }
}
