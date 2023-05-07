using CapaEntidad;
using CapaLogica;
using Xunit;

namespace MadereraCarochoTest
{
    public class LogicaCompraTest
    {
        [Fact]
        public void CrearCompraTest()
        {
            //  Setup
            entUsuario u = new entUsuario();
            u.IdUsuario = 1;
            //entCompra c = new entCompra(-4234M, true, u);
            entCompra c = new entCompra
            {
                Usuario = u,
                Total = -58,
                Estado = false
            };
            //entCompra c = new entCompra { 
            //    Total = 1,
            //    Estado = true,
            //};

            //  Action
            int idCompra = -1;
            bool isValid = logCompra.Instancia.CrearCompra(c, out idCompra);
            //  Assert
            Assert.True(isValid, $"isValid={isValid}, idCompra={idCompra}");
        }
    }
}
