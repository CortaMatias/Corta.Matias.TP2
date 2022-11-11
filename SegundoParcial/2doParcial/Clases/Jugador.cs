using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial
{
    public class Jugador 
    {
        Usuario usuario;
        private List<Carta> mano;
        private List<Carta> cartasJugadas;
        private int envidoJugadorCantado = -1;
        private bool esMano;
        private int puntaje;



        public Jugador(Usuario usuario)         {
            this.Usuario = usuario;
            this.mano = new List<Carta>();
            this.cartasJugadas = new List<Carta>();
        }



        public List<Carta> Mano { get => mano; set => mano = value; }
        public List<Carta> CartasJugadas { get => cartasJugadas; set => cartasJugadas = value; }
        public bool EsMano { get => esMano; set => esMano = value; }
        public int Puntaje { get => puntaje; set => puntaje = value; }
        public Usuario Usuario { get => usuario; set => usuario = value; }
        public int EnvidoJugadorCantado { get => envidoJugadorCantado; set => envidoJugadorCantado = value; }


        public override string ToString()
        {
            return $" {this.Usuario.NickName}";
        }


        #region #Envido
        public int calcularEnvido()
        {
            int envido = 0;
            List<Carta> mano = this.mano;
            for (int i = 0; i < mano.Count; i++)
            {
                int m1 = i % (mano.Count + 1);
                int m2 = (i + 1) % (mano.Count);
                if (mano[m1].Tipo.Equals(mano[m2].Tipo))
                {
                    int tempEnvido = 20;

                    if (mano[m1].Numero < 10) tempEnvido += mano[m1].Numero;
                    if (mano[m2].Numero < 10) tempEnvido += mano[m2].Numero;
                    if (tempEnvido > envido) envido = tempEnvido;
                }
            }
            if (envido == 0)
            {
                for (int i = 0; i < mano.Count; i++)
                {
                    int envidoUnaCarta = mano[i].Numero;
                    if (envidoUnaCarta < 10 && envidoUnaCarta > envido) envido = envidoUnaCarta;
                }
            }
            return envido;
        }


        public int DecidirEnvido(int estado, Jugador jugadorContrario)
        /*  Estados :  0  --> No quiero/No canto nada
         *  1 --> Envido
         *  2 --> Envido - Envido
         *  3 --> Real Envido
         *  4 --> Falta Envido
         *  El retorno determina si acepto o no, si Retorno == estado --> quiero decir que acepto
         */
        {
            int decision = 0;
            int obligado = estado; // Obligado es lo que el jugador va a aceptar si o si en determinada situacion

            Random random = new Random();

            if (calcularEnvido() <= 23)
            {
                if (random.Next(10) == 1)
                {
                    decision = estado + 1;
                    return decision;
                }
            }

            if (estado == 0 && calcularEnvido() > 25) obligado = 1;  // Más de 25 de envido y no se cantó, obliga a cantar

            if (calcularEnvido() > 30)
            {
                if (estado == 4) return 4;
                decision = obligado + random.Next(4 - obligado);
            }
            else if (calcularEnvido() > 27)
            {
                if (estado == 3) decision = 3;
                else if (estado < 4) decision = obligado + random.Next(3 - obligado);
                else decision = 0;
            }
            else if (calcularEnvido() > 25)
            {
                if (estado < 3)
                {
                    if (estado == 2) decision = 2;
                    else decision = obligado + random.Next(2 - obligado);
                }
                else decision = 0;
            }
            else if (calcularEnvido() > 23)
            {
                switch (estado)
                {
                    case 1:
                        decision = 1;
                        break;
                    case 0:
                        decision = random.Next(1 - estado);
                        break;
                    default:
                        decision = 0;
                        break;
                }
            }

            if (estado > 0 && jugadorContrario.Puntaje == 14) //Si se canta envido y el jugador le falta un punto para ganar acepta si o si;
            {
                if (estado < 3) decision = estado + 1;
                else decision = estado;
            }

            if (decision < estado && decision != 0) return 0; //Si la respuesta es menor al nivel del que esta el estado, no se acepta.

            if (decision == 0 && estado >= 3 && calcularEnvido() > 27 && random.Next(2) == 0) return estado;

            if (estado == 0 && decision == 0 && this.esMano == false && random.Next(5) == 3) return 1;

            return decision;
        }

        #endregion





        #region #Truco      
        //public int decidirTruco(int estado, Jugador jugadorContrario)
        //{
        //    /*
        //    Estados:
        //      0 --> No quiero / No se cantó
        //      1 --> Truco
        //      2 --> Retruco
        //      3 --> Vale 4
        //    *** Si devuelve el mismo numero que se le paso a la funcion, se le interpreta como un quiero
        //    */

        //    Random random = new Random();

        //    int cantCartasTiradas = jugadorContrario.CartasJugadas.Count + cartasJugadas.Count;

        //    switch (cantCartasTiradas)
        //    {
        //        case 0:
        //        case 1: // Primera mano
        //            if (estado == 0)            // Si no se canto nada..
        //                return 0;                // .. No cantar.
        //            else
        //            {                     // Pero si me cantaron..
        //                if (CalcularCartasBuenas() >= 1 && CalcularCartasMedianas() >= 1) // .. Solo aceptar si tengo más de una buena carta y una media
        //                    return estado;
        //            }
        //            break;
        //        case 4:
        //        case 5: // Tercera Mano
        //                // Si esta en la ultima mano y solo falto tirar yo
        //            if (jugadorContrario.CartasJugadas.Count - 1 == cartasJugadas.Count)
        //            {
        //                if (mano[0].rankingCartas() > jugadorContrario.CartasJugadas[2].rankingCartas()) // Si le gano, canto
        //                    return estado;
        //                // Si le empata, pero gano la primera
        //                else if (mano[0].rankingCartas() > jugadorContrario.CartasJugadas[2].rankingCartas() && jugadorContrario.CartasJugadas[2].rankingCartas() == mano[0].rankingCartas())
        //                    return estado + random.nextInt(4 - estado);
        //            }
        //            else if (cartasJugadas.size() == 3 && cartasJugadas.get(2).rankingCarta() > 9)
        //            { // Si queda una buena carta
        //                if (estado == 3)
        //                    return 3;
        //                return estado + random.nextInt(3 - estado);      // Apostar todo
        //            }
        //            break;
        //        case 2:
        //        case 3: // Segunda mano
        //            if (cantBuenasCartas() > 1)
        //            {  // Si tengo mas de una buena carta apostar todo
        //                if (estado == 3)
        //                    return 3;
        //                return estado + random.nextInt(3 - estado);
        //            }
        //            // Si empaté la anterior...
        //            if (p.getCartasJugadas().get(p.getCartasJugadas().size() - 1).rankingCarta() == cartasJugadas.get(cartasJugadas.size() - 1).rankingCarta())
        //            {
        //                if (cantBuenasCartas() >= 1)
        //                {
        //                    if (estado == 3)
        //                        return 3;
        //                    return estado + random.nextInt(3 - estado);
        //                }
        //            }
        //            break;
        //    }
        //    return 0;
        //}


        public Carta JugarTurno(Jugador jugadorContrario)
            {
            if (jugadorContrario.CartasJugadas.Count == 0) //Si soy el primero.
                return TirarRandom();


            if(jugadorContrario.CartasJugadas.Count == 1 && cartasJugadas.Count == 0)
            {
                for (int i = 0; i < mano.Count; i++) //Recorro de la peor a la mejor carta buscando si hay una carta que empata la que ya tiro
                {       if ((jugadorContrario.CartasJugadas.Count >= 2) && jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() == mano[i].rankingCartas())
                        return TirarCartaIndex(i); //Si emparda la juega

                        if (jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() > mano[i].rankingCartas()) return tirarPeorCarta();
                }
                if (jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() < mano[0].rankingCartas()) return TirarCartaIndex(0); //Nuevo
                if (jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() < mano[1].rankingCartas()) return TirarCartaIndex(1); //Nuevo


            }

            if (cartasJugadas.Count >= 1) // Si ya jugamos una o mas cartas
            {           

                if (jugadorContrario.CartasJugadas[0].rankingCartas() == cartasJugadas[0].rankingCartas())
                    return tirarMejorCarta(); //Y la primera fue parda jugar la mejor siempre.

                if (jugadorContrario.CartasJugadas[0].rankingCartas() > cartasJugadas[0].rankingCartas()) //Si gano primera el contrario
                {
                    for (int i = 0; i < mano.Count; i++) //Recorro de la peor a la mejor carta buscando si hay una carta que empata la que ya tiro
                    {
                           if (jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() < mano[i].rankingCartas())
                            return TirarCartaIndex(i); //Si gana la tira

                        if ((jugadorContrario.CartasJugadas.Count >= 2 ) && jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() == mano[i].rankingCartas())
                            return TirarCartaIndex(i); //Si emparda la juega                        
                    }
                    if (jugadorContrario.CartasJugadas[jugadorContrario.CartasJugadas.Count - 1].rankingCartas() < mano[0].rankingCartas()) return TirarCartaIndex(0); //Nuevo
                    return tirarMejorCarta(); //Tira la mejor.
                }

                if (jugadorContrario.CartasJugadas[0].rankingCartas() < cartasJugadas[0].rankingCartas()
                    && cartasJugadas.Count == 1) return tirarPeorCarta();

            }
            return tirarMejorCarta(); //Tira la carta que le queda;
        }

        public string MostrarCartas()
        {
            string mensaje = "";
            mensaje +=this.mano[0].ToString()+ " -- "; 
            mensaje += this.mano[1].ToString()+ " -- ";
            mensaje += this.mano[2].ToString() + " -- ";

            return mensaje;
        }


        

        /// <summary>
        /// Devuelve la carta que se va a tirar a la mesa
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public Carta tirarCarta(int indice)
        {
            Carta aTirar = mano[indice];
            mano.Remove(aTirar);
            cartasJugadas.Add(aTirar);
            return aTirar;
        }

        public void ordenarMano()
        {
            List<Carta> temporal = new();

            Mano.Sort((carta1, carta2) => carta1.rankingCartas() < carta2.rankingCartas() ? -1 : 1);

            //for (int i = 0; i < 3; i++)
            //{
            //    int smallIndex = 0;
            //    for (int j = 1; j < mano.Count; j++)
            //    {
            //        if (mano[j].rankingCartas() > mano[smallIndex].rankingCartas()) smallIndex = j;
            //    }
            //    temporal.Add(mano[smallIndex]);
            //    mano.Remove(mano[smallIndex]);
            //}
            //mano = temporal;
        }

        public Carta TirarRandom()
        {
            int index = new Random().Next(1, 3);
            Carta aTirar = mano[index];
            mano.RemoveAt(index);
            cartasJugadas.Add(aTirar);
            return aTirar;
        }


        public Carta TirarCartaIndex(int indice)
        {
            Carta aTirar = mano[indice];
            mano.RemoveAt(indice);
            cartasJugadas.Add(aTirar);
            return aTirar;
        }

        public Carta tirarMejorCarta()
        {
            if (mano.Count == 0) return null;

            int mejor = 0;
            for (int i = 1; i < mano.Count; i++)
            {
                if (mano[mejor].rankingCartas() < mano[i].rankingCartas())
                    mejor = i;
            }

            Carta aTirar = mano[mejor];
            mano.RemoveAt(mejor);
            cartasJugadas.Add(aTirar);
            return aTirar;
        }

        public Carta tirarPeorCarta()
        {
            if (mano.Count == 0) return null;

            int mejor = 0;
            for (int i = 1; i < mano.Count; i++)
            {
                //Lo mismo que mejor pero damos vuelta la condicicion
                if (mano[mejor].rankingCartas() > mano[i].rankingCartas())
                    mejor = i;
            }

            Carta aTirar = mano[mejor];
            mano.RemoveAt(mejor);
            cartasJugadas.Add(aTirar);
            return aTirar;
        }

        public int CalcularCartasBuenas() //Devuelve la cantidad de cartas "buenas" que tiene el jugador
        {
            int cantidad = 0;

            for (int i = 0; i < mano.Count; i++)
                if (mano[i].rankingCartas() > 7) cantidad++;
            return cantidad;
        }

        public int CalcularCartasMedianas()  //Devuelve la cantidad de cartas medianas que tiene el jugador
        {
            int cantidad = 0;

            for (int i = 0; i < mano.Count; i++)
                if (mano[i].rankingCartas() > 4 && mano[i].rankingCartas() < 8) cantidad++;
            return cantidad;

        }
        #endregion
    }
}
