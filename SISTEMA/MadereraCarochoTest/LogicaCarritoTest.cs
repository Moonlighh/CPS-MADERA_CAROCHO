using CapaEntidad;
using CapaLogica;
using Xunit;

namespace MadereraCarochoTest
{
    public class LogicaCarritoTest
    {
        /*
            Para que la prueba tenga �xito, es importante que sigas los siguientes puntos:
            1.La prueba unitaria asume que los datos que est�s utilizando son v�lidos y que se pueden insertar en la base de datos.Si no es as�, la prueba fallar�.
            2.La prueba unitaria no garantiza que el c�digo que est�s probando es 100% correcto, pero s� te permitir� detectar errores en la funcionalidad que est�s probando.
            3.Es importante que ejecutes esta prueba en un ambiente controlado, como una base de datos de prueba, para evitar afectar la informaci�n de producci�n.
            4.Si en alg�n momento actualizas la funcionalidad de AgregarProductoCarrito, es importante que actualices tambi�n la prueba unitaria correspondiente para asegurarte de que sigue funcionando correctamente.
            5. Una vez que tengas en cuenta estos puntos, puedes ejecutar tu prueba unitaria para validar la funcionalidad de la funci�n AgregarProductoCarrito. Si la prueba tiene �xito, podr�s estar seguro de que la capa de l�gica est� funcionando correctamente en relaci�n a esta funci�n en particular.
         */
        [Fact]
        public void AgregarProductoCarritoTest()
        {
            /*
             * Setup:
                La configuraci�n se encarga de establecer el ambiente necesario para realizar la prueba, como instanciar 
                objetos, inicializar variables, crear bases de datos de prueba, entre otros.
             */
            entRoll rol = new entRoll
            {
                IdRoll = 1
            };
            entUsuario u = new entUsuario
            {
                IdUsuario = 1,
                Activo = false,
                Roll = rol
            };
            /*
             * Act:
                La ejecuci�n del m�todo que se est� probando es el acto de llamar al m�todo que se quiere probar, con los argumentos necesarios.
            */
            bool resultado = logCarrito.Instancia.AgregarProductoCarrito(u, 1, -78);
            /*
             * Act:
                Validaci�n de los resultados se encarga de comparar los resultados esperados con los resultados reales que se obtuvieron al ejecutar 
                el m�todo probado. Si los resultados no coinciden, la prueba falla y se deben corregir los errores encontrados.
            */
            Assert.True(resultado);
        }

        [Fact]
        public void EditarProductoCarritoTest()
        {
            //Arrange
            entCarrito c = new entCarrito
            {
                IdCarrito = 4,
                Cantidad = 1,
                Subtotal = 12345678.16M
            };
            //Act
            bool isValid = logCarrito.Instancia.EditarProductoCarrito(c);
            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void EliminarProductoCarritoTest()
        {
            //Arrange
            int idProvProd = 0;
            int idCliente = -896;
            //Act
            bool isValid = logCarrito.Instancia.EliminarProductoCarrito(idProvProd, idCliente);
            //Assert
            Assert.False(isValid);
        }

        [Fact]
        public void CrearCompraTest()
        {
            entUsuario u = new entUsuario();
            u.IdUsuario = -90;
            int idGenerado = -1;
            //Arrange
            var compra = new entCompra
            {
                Usuario = u,
                Estado = true,
                Total = -5435534
            };
            //Act
            bool isValid = logCompra.Instancia.CrearCompra(compra, out idGenerado);
            //Assert
            Assert.False(isValid);
        }
    }
}