using _2doParcial;
using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            //Jugador jugador1 = new(new Usuario(2, "alfredo", 3, 4));
            //Jugador jugador2 = new(new Usuario(3, "Juan", 5, 2));
            //List<Jugador> listaJugadores = new();
            //listaJugadores.Add(jugador1);
            //listaJugadores.Add(jugador2);
            //List<Jugador> mano = listaJugadores.Where(x => x.EsMano == true).ToList();
            //Console.WriteLine(mano);
            Random random = new();
            Console.WriteLine(random.Next(0, 2));
            Console.WriteLine(random.Next(0, 2));
            Console.WriteLine(random.Next(0, 2));
            Console.WriteLine(random.Next(0, 2));
            Console.WriteLine(random.Next(0, 2));

            Console.WriteLine(random.Next(0, 2));
            Console.WriteLine(random.Next(0, 2));


        }
    }
}
