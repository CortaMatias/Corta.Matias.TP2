using _2doParcial;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestSala
{
    [TestClass]
    public class Sala_Test
    {

        [TestMethod]
        public void Test_GenerarMazo_Sucess()
        {
            Jugador j1 = new(new _2doParcial.Clases.Usuario(1, "Pablo", 5, 1));
            Jugador j2 = new(new _2doParcial.Clases.Usuario(0, "Alfredo", 20, 5));
            j1.EsMano = true;


            Sala sala = new(j1, j2);
            sala.Mazo = sala.GenerarMazo();
            
            Assert.IsTrue(sala.Mazo.Count == 40);
        }

        [TestMethod]
        public void Test_MezclarMazo_Succes()
        {
            Jugador j1 = new(new _2doParcial.Clases.Usuario(1, "Pablo", 5, 1));
            Jugador j2 = new(new _2doParcial.Clases.Usuario(0, "Alfredo", 20, 5));
            Sala sala = new Sala(j1, j2);
            sala.Mazo = sala.GenerarMazo();
            List<Carta> original = sala.Mazo;
            sala.MezclarMazo();
            List<Carta> mazoMezclado = sala.Mazo;
            Assert.AreNotEqual(original, mazoMezclado);
        }      

        [TestMethod]
        public void Test_Repartir_Sucess()
        {
            Jugador j1 = new(new _2doParcial.Clases.Usuario(1,"Pablo",5,1));
            Jugador j2 = new(new _2doParcial.Clases.Usuario(0,"Alfredo", 20,5));
            j1.EsMano = true;


            Sala sala = new(j1, j2);
            sala.CargarMazo();
            sala.MezclarMazo();
            sala.Repartir();

            Assert.IsTrue(j1.Mano.Count + j2.Mano.Count == 6);
            Assert.AreNotEqual(j1.Mano, j2.Mano);
        }

        [TestMethod]
        public void Test_JugarRonda_Sucess()
        {
            Jugador j1 = new(new _2doParcial.Clases.Usuario(1, "Pablo", 5, 1));
            Jugador j2 = new(new _2doParcial.Clases.Usuario(0, "Alfredo", 20, 5));
            j1.EsMano = true;
            Sala sala = new(j1, j2);
            sala.ResetearStats();
            sala.CargarMazo();
            sala.MezclarMazo();
            sala.Repartir();

            sala.JugarRonda(j1,j2,1);
            Assert.IsTrue(j1.Mano.Count == 2);
            Assert.IsTrue(j2.Mano.Count == 2);
            Assert.IsTrue(j2.CartasJugadas.Count == 1);
            Assert.IsTrue(j1.CartasJugadas.Count == 1);
            Assert.IsTrue(sala.Mesa.Count == 2);
        }

        [TestMethod]
        public void Test_JugarMano_Sucess()
        {
            Jugador j1 = new(new _2doParcial.Clases.Usuario(1, "Pablo", 5, 1));
            Jugador j2 = new(new _2doParcial.Clases.Usuario(0, "Alfredo", 20, 5));
            j1.EsMano = true;
            Sala sala = new(j1, j2);
            sala.ResetearStats();
            sala.CargarMazo();
            sala.MezclarMazo();
            sala.Repartir();
            sala.Mano = j1;
            sala.Pie = j2;
            

            sala.JugarMano();
            Assert.IsTrue(j1.Mano.Count <= 3);
            Assert.IsTrue(j2.Mano.Count <= 3);
            Assert.IsTrue(j2.CartasJugadas.Count > 0);
            Assert.IsTrue(j1.CartasJugadas.Count > 0);
            Assert.IsTrue(sala.Mesa.Count >= 2);
            Assert.IsTrue(sala.GanadorMano != null);
            Assert.IsTrue(sala.GanadorMano.Puntaje != 0);
            Assert.IsTrue(sala.TurnosJugados != 0);
            if (sala.NivelEnvido != 0) Assert.IsTrue(sala.JGanadorEnvido.Puntaje != 0);            
            if (sala.NivelTruco != 0) Assert.IsTrue(sala.GanadorMano.Puntaje >= 2);
            


        }




    }
}
