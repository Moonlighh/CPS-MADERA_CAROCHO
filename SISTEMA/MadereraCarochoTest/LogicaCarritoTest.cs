using CapaEntidad;
using CapaLogica;
using Xunit;

namespace MadereraCarochoTest
{
    public class LogicaCarritoTest
    {
        /*
            Para que la prueba tenga éxito, es importante que sigas los siguientes puntos:
            1.La prueba unitaria asume que los datos que estás utilizando son válidos y que se pueden insertar en la base de datos.Si no es así, la prueba fallará.
            2.La prueba unitaria no garantiza que el código que estás probando es 100% correcto, pero sí te permitirá detectar errores en la funcionalidad que estás probando.
            3.Es importante que ejecutes esta prueba en un ambiente controlado, como una base de datos de prueba, para evitar afectar la información de producción.
            4.Si en algún momento actualizas la funcionalidad de AgregarProductoCarrito, es importante que actualices también la prueba unitaria correspondiente para asegurarte de que sigue funcionando correctamente.
            5. Una vez que tengas en cuenta estos puntos, puedes ejecutar tu prueba unitaria para validar la funcionalidad de la función AgregarProductoCarrito. Si la prueba tiene éxito, podrás estar seguro de que la capa de lógica está funcionando correctamente en relación a esta función en particular.
         */
        [Fact]
        public void AgregarProductoCarritoTest()
        {
            /*
             * Setup:
                La configuración se encarga de establecer el ambiente necesario para realizar la prueba, como instanciar 
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
                La ejecución del método que se está probando es el acto de llamar al método que se quiere probar, con los argumentos necesarios.
            */
            bool resultado = logCarrito.Instancia.AgregarProductoCarrito(u, 1, -78);
            /*
             * Act:
                Validación de los resultados se encarga de comparar los resultados esperados con los resultados reales que se obtuvieron al ejecutar 
                el método probado. Si los resultados no coinciden, la prueba falla y se deben corregir los errores encontrados.
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