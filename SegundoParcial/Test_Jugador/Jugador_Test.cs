using _2doParcial;
using _2doParcial.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_Jugador
{
    [TestClass]
    public class Jugador_Test
    {

        [TestMethod]
        public void Test_CalcularEnvido_Sucess()
        {
            Jugador j1 = new(new Usuario(2,"alfredo",3,4));
            Jugador j2 = new(new Usuario(3, "Juan", 5,2));
            j1.Mano.Add(new Carta(6, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "copa"));
            int envido=j1.calcularEnvido();
            Assert.AreEqual(33,envido);
        }


        [TestMethod]
        public void Test_JugarTurno_Sucess()
        {
        

        }


    }
}
