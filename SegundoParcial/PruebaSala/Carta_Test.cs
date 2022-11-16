using _2doParcial;
using _2doParcial.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEntidades
{
    [TestClass]
    public class Carta_Test
    {
        [TestMethod]
        public void Test_RankignCartas_Sucess()
        {
            Carta c1 = new Carta(1, "espada");
            int rank = c1.rankingCartas();
            Assert.IsTrue(rank == 13);            
        }


        [TestMethod]
        public void Test_RankignCartas2_Sucess()
        {
            Carta c1 = new Carta(4, "copa");
            int rank = c1.rankingCartas();
            Assert.IsTrue(rank == 0);
        }

        [TestMethod]
        public void Test_RankignCartas3_Sucess()
        {
            Carta c1 = new Carta(12, "basto");
            int rank = c1.rankingCartas();
            Assert.IsTrue(rank == 6);
        }
    }
}
