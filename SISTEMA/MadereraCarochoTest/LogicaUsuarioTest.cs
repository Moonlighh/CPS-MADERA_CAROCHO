using CapaAccesoDatos;
using CapaEntidad;
using CapaLogica;
using Moq;
using Xunit;

namespace MadereraCarochoTest
{
    public class LogicaUsuarioTest
    {
        [Fact]
        public void IniciarSesionTest()
        {
            // Preparación: creo una instancia del objeto mock para la interfaz IDatUsuario y del objeto entUsuario esperado
            var mockDatUsuario = new Mock<IDatUsuario>();
            var userEsperado = new entUsuario { IdUsuario = 1, Correo = "cesar@gmail.com" };

            // Configuración: le indico al objeto mock cómo debe comportarse al recibir los datos "Cesar" y "123", y le digo que devuelva el objeto esperado
            mockDatUsuario.Setup(x => x.IniciarSesion("Cesar", "123")).Returns(userEsperado);

            // Ejecución: creo una instancia del objeto logUsuario con el objeto mock como parámetro, llamo al método IniciarSesion con los mismos datos, y guardo el resultado en la variable result
            var sut = new logUsuario(mockDatUsuario.Object);
            var result = sut.IniciarSesion("Cesar", "123");

            // Validación: compruebo que el resultado obtenido sea igual al objeto esperado
            Assert.Equal(userEsperado, result);
        }
        [Fact]
        public void IniciarSesionTest2()
        {
            logUsuario lUser = new logUsuario(new datUsuario());

            var result = lUser.IniciarSesion("Cesar", "123");

            Assert.True(result != null);
        }
        [Fact]
        public void CrearCliente()
        {
            var mockDatUsuario = new Mock<IDatUsuario>();
            var valorEsperado = true;
            entUsuario user = new entUsuario("Anticona", "reyesanticona25@gmail.com", "CesarRuben", new entRoll { IdRoll = 2});                     
            mockDatUsuario.Setup(x => x.CrearSesionUsuario(user)).Returns(valorEsperado);
            List<string> lsErrores = new List<string>();
            var sut = new logUsuario(mockDatUsuario.Object);
            var result = sut.CrearSesionUsuario(user, out lsErrores);

            Assert.True(result && lsErrores.Count == 0);
        }
    }
}
