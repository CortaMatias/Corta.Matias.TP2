using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    class Ronda
    {
        private Jugador ganador;
        private Carta ganadora;
        private Jugador mano;


        public Jugador Ganador { get => ganador; set => ganador = value; }
        public Carta Ganadora { get => ganadora; set => ganadora = value; }
        public Jugador Mano { get => mano; set => mano = value; }


    }
}
