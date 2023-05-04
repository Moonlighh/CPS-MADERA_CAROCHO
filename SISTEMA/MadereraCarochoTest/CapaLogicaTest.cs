using CapaEntidad;
using CapaLogica;
using Xunit;

namespace MadereraCarochoTest
{
    public class CapaLogicaTest
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
            // Arrange
            entUsuario u = new entUsuario
            {
                IdUsuario = 1
            };
            entProveedorProducto proveedorProducto = new entProveedorProducto
            {
                IdProveedorProducto = 1
            };
            entCarrito c = new entCarrito
            {
                Cliente = u,
                ProveedorProducto = proveedorProducto,
                Cantidad = 120,
                Subtotal = 2133
            };

            // Act
            bool resultado = logCarrito.Instancia.AgregarProductoCarrito(c);

            // Assert
            Assert.True(resultado);
        }
    }
}