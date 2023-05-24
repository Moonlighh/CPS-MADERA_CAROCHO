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
        public void CrearCliente()
        {
            // Crear el mock(para luego simular el comportamiento del objeto)
            var mockDatUsuario = new Mock<IDatUsuario>();
            var valorEsperado = true;

            // Creo los datos de prueba
            string usuario = "Chuben";
            string correo = "reyesanticona25@gmail.com";
            string pass = "R*-2";
            entUsuario user = new entUsuario
            {
                UserName = usuario,
                Correo = correo,
                Pass = pass,
            };
            // Simular el comportamiento del objeto
            mockDatUsuario.Setup(x => x.CrearSesionUsuario(user)).Returns(valorEsperado);

            List<string> lsErrores = new List<string>();
            var sut = new logUsuario(mockDatUsuario.Object);
            var result = sut.CrearSesionUsuario(usuario, correo, pass, out lsErrores);

            Assert.True(result);
        }
    }
}
