using _2doParcial;
using _2doParcial.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSala
{
    [TestClass]
    public class Jugador_Test
    {        
            [TestMethod]
            public void Test_CalcularEnvido_Sucess()
            {
                Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));                
                j1.Mano.Add(new Carta(6, "espada"));
                j1.Mano.Add(new Carta(7, "espada"));
                j1.Mano.Add(new Carta(4, "espada"));
                int envido = j1.calcularEnvido();
                Assert.AreEqual(33, envido);
            }


            [TestMethod]
            public void Test_calcularCartasBuenas()
            {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            int buenasCartas = j1.CalcularCartasBuenas();
            Assert.AreEqual(buenasCartas , 3);
            }


        [TestMethod]
        public void Test_calcularCartasBuenas2()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(4, "espada"));
            j1.Mano.Add(new Carta(6, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            int buenasCartas = j1.CalcularCartasBuenas();
            Assert.AreEqual(buenasCartas, 0);
        }

        [TestMethod]
        public void Test_calcularCartasMedianas()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(10, "espada"));
            j1.Mano.Add(new Carta(11, "espada"));
            j1.Mano.Add(new Carta(12, "basto"));
            int medianasCartas = j1.CalcularCartasMedianas();
            Assert.AreEqual(medianasCartas, 3);
        }

        [TestMethod]
        public void Test_calcularCartasMedianas2()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            int medianasCartas = j1.CalcularCartasMedianas();
            Assert.AreEqual(medianasCartas, 0);
        }

        [TestMethod]
        public void Test_TirarPeorCarta()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            Carta carta = j1.tirarPeorCarta();
            Assert.AreEqual(carta, j1.CartasJugadas[0]);
        }

        [TestMethod]
        public void Test_TirarPeorCarta1()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(4, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            Carta carta = j1.tirarPeorCarta();
            Assert.AreEqual(carta, j1.CartasJugadas[0]);
        }


        [TestMethod]
        public void Test_TirarMejorCarta()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(4, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            Carta carta = j1.tirarMejorCarta();
            Assert.AreEqual(carta, j1.CartasJugadas[0]);
        }

        [TestMethod]
        public void Test_TirarMejorCarta1()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            Carta carta = j1.tirarMejorCarta();
            Assert.AreEqual(carta, j1.CartasJugadas[0]);
        }

        [TestMethod]
        public void Test_TirarCartaIndex()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            Carta carta = j1.TirarCartaIndex(0);
            Assert.AreEqual(carta, new Carta(1, "espada"));
        }

        [TestMethod]
        public void Test_TirarCartaIndex2()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            Carta carta = j1.TirarCartaIndex(2);
            Assert.AreEqual(carta, new Carta(4, "basto"));
        }


        [TestMethod]
        public void Test_TirarRandom()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            Carta carta = j1.TirarRandom();
            Assert.IsTrue(j1.CartasJugadas.Contains(carta));
        }

        [TestMethod]
        public void Test_TirarRandom2()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
            Carta carta = j1.TirarRandom();
            Assert.IsFalse(j1.Mano.Contains(carta));
        }


        [TestMethod]
        public void Test_OrdenarMano()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(4, "basto"));
             j1.ordenarMano();
            Assert.IsTrue(j1.Mano[0] == new Carta(4, "basto"));
        }


        [TestMethod]
        public void Test_OrdenarMano2()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            j1.ordenarMano();
            Assert.IsTrue(j1.Mano[2] == new Carta(1, "espada"));
        }

        [TestMethod]
        public void Test_OrdenarMano3()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            j1.ordenarMano();
            Assert.IsTrue(j1.Mano[0] == new Carta(7, "espada"));
        }

        [TestMethod]
        public void Test_JugarTurno()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(1, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            Carta cartaJugada = j1.JugarTurno(j2);
            Assert.IsTrue(j1.Mano.Count == 2);
            Assert.IsTrue(j1.CartasJugadas.Count== 1);
            Assert.IsTrue(j1.CartasJugadas[0] == cartaJugada);
            Assert.IsTrue(!j1.Mano.Contains(cartaJugada));
        }

        [TestMethod]
        public void Test_DecidirEnvido()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(6, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            int nivelEnvido = j1.DecidirEnvido(0,j2);
            Assert.IsTrue(nivelEnvido >= 1);            
        }

        [TestMethod]
        public void Test_DecidirEnvido1()
        {
            Jugador j1 = new(new Usuario(2, "alfredo", 3, 4));
            Jugador j2 = new(new Usuario(3, "Juan", 5, 2));
            j1.Mano.Add(new Carta(6, "espada"));
            j1.Mano.Add(new Carta(7, "espada"));
            j1.Mano.Add(new Carta(1, "basto"));
            int nivelEnvido = j1.DecidirEnvido(1, j2);
            Assert.IsTrue(nivelEnvido >= 1);
        }

    }
}
