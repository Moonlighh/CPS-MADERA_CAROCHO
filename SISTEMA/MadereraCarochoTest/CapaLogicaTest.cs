using CapaEntidad;
using CapaLogica;
using Xunit;

namespace MadereraCarochoTest
{
    public class CapaLogicaTest
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