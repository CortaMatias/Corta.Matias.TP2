using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2doParcial
{
    public class Sala
    {
        private string log;
        private Task tarea;
        private CancellationToken token;
        public event EventHandler actualizar;
        public event EventHandler eventoFinalizarPartida;
        public event EventHandler eventoCargarDatosUsuario;
        public event EventHandler eventoCargarDatosPartidas;

        private Jugador jugador1;
        private Jugador jugador2;
        private Jugador mano;
        private Jugador pie;
        private Jugador jGanadorEnvido;
        private Jugador jPerdedorEnvido;
        private Jugador ganadorMano;
        private Jugador ultimoMano;

        private List<Carta> mazo;
        private List<Carta> mesa;
        private List<int> envidosCantados;

        private bool terminoPartida = false;
        private bool envidoFinalizado = false;
        private bool trucoCantado = false;
        private int nivelTruco = 0;
        private int nivelEnvido = 0;
        private int turnosJugados;
        private int manosJugadas;
        private int parda;

        

        public Jugador Jugador1 { get => jugador1; set => jugador1 = value; }
        public Jugador Jugador2 { get => jugador2; set => jugador2 = value; }
        public List<Carta> Mazo { get => mazo; set => mazo = value; }
        public string Log { get => log; set => log = value; }
        public Task Tarea { get => tarea; set => tarea = value; }
        public CancellationToken Token { get => token; set => token = value; }
        public bool TerminoPartida { get => terminoPartida; set => terminoPartida = value; }
        public List<Carta> Mesa { get => mesa; set => mesa = value; }
        public Jugador GanadorMano { get => ganadorMano; set => ganadorMano = value; }
        public int TurnosJugados { get => turnosJugados; set => turnosJugados = value; }
        public Jugador Mano { get => mano; set => mano = value; }
        public Jugador Pie { get => pie; set => pie = value; }
        public Jugador JGanadorEnvido { get => jGanadorEnvido; set => jGanadorEnvido = value; }
        public Jugador JPerdedorEnvido { get => jPerdedorEnvido; set => jPerdedorEnvido = value; }
        public int NivelTruco { get => nivelTruco; set => nivelTruco = value; }
        public int NivelEnvido { get => nivelEnvido; set => nivelEnvido = value; }
        public bool EnvidoFinalizado { get => envidoFinalizado; set => envidoFinalizado = value; }

        public Sala(Jugador j1, Jugador j2)
        {
            Mesa = new();
            envidosCantados = new();
            this.jugador1 = j1;
            this.jugador2 = j2;
            this.mazo = new List<Carta>();
            this.log = "";
            this.Tarea = new Task(() => ComenzarJuego(), token);
            Tarea.Start();         
        }


        /// <summary>
        /// Resetea los elementos necesarios para la proxima mano.
        /// </summary>
        public void ResetearStats()
        {
            jugador1.RondasGanadas = 0;
            jugador2.RondasGanadas = 0;
            jugador1.EnvidoJugadorCantado = -1;
            jugador2.EnvidoJugadorCantado = -1;
            NivelTruco = 0;
            NivelEnvido = 0;
            Mesa.Clear();
            GanadorMano = null;
            envidosCantados.Clear();
            EnvidoFinalizado = false;
            jugador1.Mano.Clear();
            jugador2.Mano.Clear();
            jugador1.CartasJugadas.Clear();
            jugador2.CartasJugadas.Clear();
            parda = 0;
            trucoCantado = false;
        }

        public void InvocarEventoPartida(string mensaje)
        {
            log = log.Insert(0,mensaje);
            actualizar?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return $"{jugador1.ToString()} VS {jugador2.ToString()}";
        }

     


        /// <summary>
        /// Los jugadores juegan un turno cada uno y devuelve el jugador ganador de la ronda
        /// </summary>
        /// <param name="jMano"></param>
        /// <param name="jNoMano"></param>
        /// <param name="turno"></param>
        /// <returns></returns>
        public Jugador JugarRonda(Jugador jMano, Jugador jNoMano, int turno)
        {
            Jugador ganador = jMano;
            jMano.EsMano = true;
            jNoMano.EsMano = false;
            int decision;          

            // ======================== jMano ============================
            if (turno == 1 && !EnvidoFinalizado) EnvidoMano(jMano, jNoMano);

            decision = jMano.DecidirTruco();
            if(decision == 1 && !trucoCantado &&  turno ==2)
            {
                trucoCantado = true;
                InvocarEventoPartida($"{jMano.ToString()}: Truco! \n\n");
                JugarTruco(jMano,jNoMano);
            }
          
            if (GanadorMano == null)
            {
                Mesa.Add(jMano.JugarTurno(jNoMano));
                InvocarEventoPartida($" {jMano.ToString()}: jugo {Mesa[Mesa.Count - 1].ToString()} \n\n");

                Thread.Sleep(500);
                // ======================== jMano ============================
                // ========= NO MANO =======================
                if (turno == 1 && !EnvidoFinalizado) EnvidoNoMano(jMano, jNoMano);
                
                decision = jNoMano.DecidirTruco();
                if (decision == 1 && !trucoCantado && turno == 2)
                {
                    trucoCantado = true;
                    InvocarEventoPartida($"{jNoMano.ToString()}: Truco! \n\n");
                    JugarTruco(jNoMano, jMano);
                }

                if (GanadorMano == null)
                {
                    Mesa.Add(jNoMano.JugarTurno(jMano));
                    InvocarEventoPartida($" {jNoMano.ToString()} jugo: {Mesa[Mesa.Count - 1].ToString()} \n\n");
                    Thread.Sleep(500);
                    ganador = ganadorRonda(jMano, jNoMano);
                }
                else return null;
                // ========= NO MANO =======================
               
            }
            else return null;
                TurnosJugados++;
            
            return ganador;
        } 



        /// <summary>
        /// Jugar mano hace que los jugadores jueguen las respectivas rondas(cada uno un turno ) y seteen al ganadorMano
        /// </summary>
        public void JugarMano()
        {

            int puntaje = 1;
            Mano.CantCartasBuenas = Mano.CalcularCartasBuenas();
            Pie.CantCartasBuenas = Pie.CalcularCartasBuenas();
            this.GanadorMano = null;
            Jugador ganadorRonda = JugarRonda(Mano, Pie, 1); //PrimerRonda

            if (jugador1.Puntaje >= 15 || jugador2.Puntaje >= 15)
            {
                TerminoPartida = true;
                return;
            }

            if (parda == 1) //parda a la mejor
            {
                this.GanadorMano = JugarRonda(Mano, Pie, 2); //SegundaRonda
            }

            if (jugador1.Puntaje >= 15 || jugador2.Puntaje >= 15)
            {
                TerminoPartida = true;
                return;
            }

            if (parda == 0) //Si no hubo parda
              {
                ganadorRonda.RondasGanadas++;
                if (Mano != ganadorRonda) { Jugador aux = Mano; Mano = Pie; Pie = aux; }
                ganadorRonda = JugarRonda(Mano, Pie, 2); //Segunda Ronda
              }

            if (parda == 1) GanadorMano = ganadorRonda;

            if (NivelTruco == 1) puntaje = 2;

            if (this.GanadorMano == null)
            {      
                    if (Mano != ganadorRonda) { Jugador aux = Mano; Mano = Pie; Pie = aux; }
                    this.GanadorMano = JugarRonda(Mano, Pie, 3); //Tercera Ronda
                    if (this.GanadorMano == null) { 
                    this.GanadorMano = Mano;
                    GanadorMano.Puntaje += puntaje;
                    InvocarEventoPartida($"{GanadorMano.ToString()} gano {puntaje} pts\n\n");
                    InvocarEventoPartida($"Ganador Mano: {GanadorMano.ToString()} \n\n"); 
                    }
                    else {
                    GanadorMano.Puntaje += puntaje;
                    InvocarEventoPartida($"{GanadorMano.ToString()} gano {puntaje} pts \n\n");
                    InvocarEventoPartida($"Ganador Mano: {GanadorMano.ToString()} \n\n"); 
                    }     
            }
            else
            {
                GanadorMano.Puntaje += puntaje;
                InvocarEventoPartida($"{GanadorMano.ToString()} gano {puntaje} pts\n\n");
                InvocarEventoPartida($"Ganador Mano: {GanadorMano.ToString()} \n\n");
            }
            if (jugador1.Puntaje >= 15 || jugador2.Puntaje >= 15)
            {
            TerminoPartida = true;
            }
                manosJugadas++;
        }       
   

        /// <summary>
        /// Inicializa las bases para comenzar el juego
        /// </summary>
        private void ComenzarJuego()  
        {          
            jugador1.Usuario.Jugando = true;
            jugador2.Usuario.Jugando = true;
            InvocarEventoPartida("");
            IniciarMano();
          
            if (TerminoPartida)
            {
                jugador2.Usuario.Jugando = false;
                jugador1.Usuario.Jugando = false;
                TerminandoPartida();
            }
            InvocarEventoPartida("");
        }



        /// <summary>
        ///
        /// </summary>
        private void IniciarMano()
        {
            Thread.Sleep(600);
            InvocarEventoPartida(" Iniciando Juego ...  \n\n");
            Thread.Sleep(500);

            while (!TerminoPartida && !token.IsCancellationRequested)
            {
                Thread.Sleep(700);
                InvocarEventoPartida($" ===== MANO {manosJugadas + 1} ===== \n\n");
                Thread.Sleep(700);
                InvocarEventoPartida(" Mezclando . . . \n\n");
                Thread.Sleep(700);

                Thread.Sleep(700);
                InvocarEventoPartida(" Repartiendo ... \n\n");
                Thread.Sleep(700);
                ResetearStats();

                mazo = GenerarMazo();
                MezclarMazo();
                Repartir();
                jugador1.ordenarMano();
                jugador2.ordenarMano();

                if (Mano == null) { Mano = Jugador1; Pie = Jugador2; }
                else if (ultimoMano == Mano)
                {
                    Jugador aux = Mano;
                    Mano = Pie;
                    Pie = aux;
                }
                ultimoMano = Mano;
                InvocarEventoPartida($"\n ** Cartas {Mano.ToString()}: {Mano.MostrarCartas()} **\n\n");
                Thread.Sleep(700);
                InvocarEventoPartida($"\n ** Cartas {Pie.ToString()}: {Pie.MostrarCartas()} ** \n\n");
                Thread.Sleep(700);
                JugarMano();
            }

            if (token.IsCancellationRequested)
            {
                terminoPartida = true;
                jugador2.Usuario.Jugando = false;
                jugador1.Usuario.Jugando = false;
                eventoFinalizarPartida?.Invoke(this, EventArgs.Empty);
            }

        }




       
        /// <summary>
        /// Hace que los jugadores jueguen el envido en caso de jMano haya cantado
        /// </summary>
        /// <param name="jMano"></param>
        /// <param name="jNoMano"></param>
        private void EnvidoNoMano(Jugador jMano, Jugador jNoMano)
        {
            NivelEnvido = jNoMano.DecidirEnvido(NivelEnvido, jMano); //Decide no mano
            if (NivelEnvido != 0 && !EnvidoFinalizado)
            {

                envidosCantados.Add(NivelEnvido); //Canta
                InvocarEventoPartida($"{jNoMano.ToString()} Canto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                Thread.Sleep(500);
                NivelEnvido = jMano.DecidirEnvido(NivelEnvido, jNoMano); //Contesta

                if (NivelEnvido == 0) //No quiere
                {
                    jNoMano.Puntaje++;
                    InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($" ***{jNoMano.ToString()} Sumo 1 punto. *** \n\n");
                    Thread.Sleep(700);
                    EnvidoFinalizado = true;
                }
                if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                {
                    InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($"{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($"{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                    Thread.Sleep(700);
                    JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                    EnvidoFinalizado = true;
                }
                if (NivelEnvido > envidosCantados[envidosCantados.Count - 1]) //Retruca
                {
                    InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                    Thread.Sleep(700);
                    envidosCantados.Add(NivelEnvido);
                    NivelEnvido = jNoMano.DecidirEnvido(NivelEnvido, jMano);
                    if (NivelEnvido == 0) //No quiere
                    {
                        jMano.Puntaje += calcularEnvidoPerdido();
                        InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                        Thread.Sleep(700);
                    }
                    if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado} \n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                        Thread.Sleep(700);
                        JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                        InvocarEventoPartida("\n");
                        Thread.Sleep(700);
                    }
                    if (NivelEnvido > envidosCantados[envidosCantados.Count - 1])
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                        Thread.Sleep(700);
                        envidosCantados.Add(NivelEnvido);
                        NivelEnvido = jMano.DecidirEnvido(NivelEnvido, jNoMano);
                        if (NivelEnvido == 0)
                        {
                            jNoMano.Puntaje += calcularEnvidoPerdido();
                            InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                            Thread.Sleep(700);
                        }
                        if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                        {
                            InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                            Thread.Sleep(700);
                            JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                            InvocarEventoPartida("\n");
                            Thread.Sleep(700);
                        }
                        if (NivelEnvido > envidosCantados[envidosCantados.Count - 1])
                        {
                            InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                            Thread.Sleep(700);
                            envidosCantados.Add(NivelEnvido);
                            NivelEnvido = jNoMano.DecidirEnvido(NivelEnvido, jNoMano);
                            if (NivelEnvido == 0)
                            {
                                jMano.Puntaje += calcularEnvidoPerdido();
                                InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                                Thread.Sleep(700);
                            }
                            if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                            {
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                                Thread.Sleep(700);
                                JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                                InvocarEventoPartida("");
                                Thread.Sleep(700);
                            }
                        }

                    }
                }
            }
        }


        /// <summary>
        /// Hace que los jugadores jueguen el envido si jNoMano canto 
        /// </summary>
        /// <param name="jMano"></param>
        /// <param name="jNoMano"></param>
        private void EnvidoMano(Jugador jMano, Jugador jNoMano)
        {

            jMano.EnvidoJugadorCantado = jMano.calcularEnvido();
            jNoMano.EnvidoJugadorCantado = jNoMano.calcularEnvido();
            NivelEnvido = jMano.DecidirEnvido(NivelEnvido, jNoMano); //Decide  mano
            if (NivelEnvido != 0 && !EnvidoFinalizado)
            {
                envidosCantados.Add(NivelEnvido); //Canta
                InvocarEventoPartida($"{jMano.ToString()} Canto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                Thread.Sleep(500);
                NivelEnvido = jNoMano.DecidirEnvido(NivelEnvido, jMano); //Contesta

                if (NivelEnvido == 0) //No quiere
                {
                    jMano.Puntaje++;
                    InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($" ***{jMano.ToString()} Sumo 1 punto. *** \n\n");
                    Thread.Sleep(500);
                    EnvidoFinalizado = true;
                }
                if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                {
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                    Thread.Sleep(500);
                    JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                    EnvidoFinalizado = true;
                }
                if (NivelEnvido > envidosCantados[envidosCantados.Count - 1]) //Retruca 1
                {
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                    Thread.Sleep(500);
                    envidosCantados.Add(NivelEnvido);
                    NivelEnvido = jMano.DecidirEnvido(NivelEnvido, jNoMano);
                    if (NivelEnvido == 0) //No quiere
                    {
                        jNoMano.Puntaje += calcularEnvidoPerdido();
                        InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                        Thread.Sleep(500);
                    }
                    if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                    {
                        InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                        Thread.Sleep(500);
                        JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                        InvocarEventoPartida("\n");
                        Thread.Sleep(500);
                    }
                    if (NivelEnvido > envidosCantados[envidosCantados.Count - 1])//--> Retruca 2
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                        Thread.Sleep(500);
                        envidosCantados.Add(NivelEnvido);
                        NivelEnvido = jNoMano.DecidirEnvido(NivelEnvido, jMano);
                        if (NivelEnvido == 0)
                        {
                            jMano.Puntaje += calcularEnvidoPerdido();
                            InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                            Thread.Sleep(500);
                        }
                        if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                        {
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                            Thread.Sleep(500);
                            JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                            InvocarEventoPartida("\n");
                            Thread.Sleep(500);
                        }
                        if (NivelEnvido > envidosCantados[envidosCantados.Count - 1]) //---> ReTruca 3
                        {
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, true)}\n\n");
                            Thread.Sleep(500);
                            envidosCantados.Add(NivelEnvido);
                            NivelEnvido = jMano.DecidirEnvido(NivelEnvido, jNoMano);
                            if (NivelEnvido == 0)
                            {
                                jNoMano.Puntaje += calcularEnvidoPerdido();
                                InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)} \n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                                Thread.Sleep(500);
                            }
                            if (NivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                            {
                                InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(NivelEnvido, false)}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                                Thread.Sleep(500);
                                JGanadorEnvido.Puntaje += calcularEnvidoGanado(JPerdedorEnvido.Puntaje);
                                InvocarEventoPartida("\n\n");
                                Thread.Sleep(500);
                            }
                        }

                    }
                }
            }
        }

 



        /// <summary>
        /// Define el ganador del envido y retorna un string con el mensaje correspondiente 
        /// </summary>
        /// <param name="j1"></param>
        /// <param name="j2"></param>
        /// <returns></returns>
        private string ganadorEnvido(Jugador j1, Jugador j2)
        {
            StringBuilder sb = new();
            if (j1.EnvidoJugadorCantado > j2.EnvidoJugadorCantado)
            {
                sb.AppendLine($"Ganador Envido: {j1.ToString()} con {j1.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{j1.ToString()} Gano: {calcularEnvidoGanado(j2.Puntaje)}pts\n\n");
                JGanadorEnvido = j1;
                JPerdedorEnvido = j2;
            }
            else if (j1.EnvidoJugadorCantado < j2.EnvidoJugadorCantado)
            {
                sb.AppendLine($"Ganador Envido: {j2.ToString()} con {j2.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{j2.ToString()} Gano: {calcularEnvidoGanado(j1.Puntaje)}pts\n\n");
                JGanadorEnvido = j2;
                JPerdedorEnvido = j1;
            }
            else if (j1.EnvidoJugadorCantado == j2.EnvidoJugadorCantado)
            {
                sb.AppendLine($"Ganador Envido: {Mano.ToString()} con {Mano.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{Mano.ToString()} Gano: {calcularEnvidoGanado(Pie.Puntaje)}pts \n\n");
                JGanadorEnvido = Mano;
                JPerdedorEnvido = Pie;
            }
            return sb.ToString();
        }


        /// <summary>
        /// Muestra el mensaje del envido segun el nivel en el que se esta jugando y si esta cantando o contestando
        /// </summary>
        /// <param name="nivelEnvido"></param>
        /// <param name="primero"></param>
        /// <returns></returns>
        private string MensajeEnvido(int nivelEnvido, bool primero)
        {
            string mensaje = "";
            switch (nivelEnvido)
            {
                case 0:
                    if (!primero) mensaje = "  No quiero!  \n\n";
                    break;
                case 1:
                    if (!primero) mensaje = "  Quiero!\n\n";
                    else mensaje = "  Envido!\n";
                    break;
                case 2:
                    if (!primero) mensaje = " Quiero! \n\n";
                    else mensaje = " Envido! \n";
                    break;
                case 3:
                    if (!primero) mensaje = " Quiero! \n\n";
                    else mensaje = " Real Envido \n\n";
                    break;
                case 4:
                    if (!primero) mensaje = " Quiero! \n\n";
                    else mensaje = " Falta Envido \n\n";
                    break;
            }
            return mensaje;
        }
        


        /// <summary>
        /// Calcula los puntos del envido ganado
        /// </summary>
        /// <param name="puntajePerdedor"></param>
        /// <returns></returns>
        private int calcularEnvidoGanado(int puntajePerdedor)
        {
            int total = 0;

            for (int i = 0; i < envidosCantados.Count; i++)
            {
                switch (envidosCantados[i])
                {
                    case 1:
                        total += 2;
                        break;
                    case 2:
                        total += 2;
                        break;
                    case 3:
                        total += 3;
                        break;
                    case 4:
                        total = 15 - puntajePerdedor;
                        break;
                }
            }
            EnvidoFinalizado = true;
            return total;
        }


        /// <summary>
        /// Calcula los puntos del envido perdido
        /// </summary>
        /// <returns></returns>
        private int calcularEnvidoPerdido()
        {
            int total = 0;

            for (int i = 0; i < envidosCantados.Count - 1; i++)
            {
                switch (envidosCantados[i])
                {
                    case 1:
                        total += 2;
                        break;
                    case 2:
                        total += 2;
                        break;
                    case 3:
                        total += 3;
                        break;
                }
            }

            if (envidosCantados.Count == 1) // Si solo se canto un envido, da un punto
                total += 1;

            EnvidoFinalizado = true;
            return total;
        }


        /// <summary>
        /// Define el gandor de la ronda segun el ranking de las cartas
        /// </summary>
        /// <param name="jugador1"></param>
        /// <param name="jugador2"></param>
        /// <returns></returns>
        public Jugador ganadorRonda(Jugador jugador1, Jugador jugador2)
        {
            int rankingJugador1 = jugador1.CartasJugadas[jugador1.CartasJugadas.Count -1].rankingCartas();
            int rankingJugador2 = jugador2.CartasJugadas[jugador2.CartasJugadas.Count - 1].rankingCartas();

            if (rankingJugador1 > rankingJugador2)
                return jugador1;
            if (rankingJugador1 < rankingJugador2)
                return jugador2;
            if(rankingJugador1 == rankingJugador2)        
                parda++;
            return null;
        }

         /// <summary>
         /// Setea a this.ganadorMano con el jugador contrario al que no acepto y si acepto setea el nivel de truco.
         /// </summary>
         /// <param name="canta"></param>
         /// <param name="contesta"></param>
        private void JugarTruco(Jugador canta, Jugador contesta)
        {
            int decision;
            decision = contesta.DecidirTruco();
            if (decision == 1)
            {
                InvocarEventoPartida($"{contesta.ToString()}: Quiero!\n\n");
                NivelTruco = 1;
                return;
            }
            if (decision == 0)
            {
                //Dijo No quiero
                InvocarEventoPartida($"{contesta.ToString()}: No Quiero!\n\n");
                GanadorMano = canta;
                NivelTruco = 0;
                return;
            }
        }


        /// <summary>
        /// Chequea si finalizo la partida e invoca al evento correspondiennte para avisar que termino, y para actualizar la informacion en la base de datos.
        /// </summary>
        private void TerminandoPartida()
        {
            DateTime fecha = DateTime.Now;
            if (jugador1.Puntaje > 15) jugador1.Puntaje = 15;
            if (jugador2.Puntaje > 15) Jugador2.Puntaje = 15;
            if (jugador1.Puntaje == 15)
            {
                jugador1.Usuario.Victorias++;
                jugador2.Usuario.Derrotas++;
                InvocarEventoPartida($"\n\nTermino la partida \n\n--GANADOR: {jugador1.ToString()} \n\n Pts {jugador1.ToString()}: {jugador1.Puntaje} " +
                $"-- Pts {jugador2.Usuario.NickName}: {jugador2.Puntaje} \n\n");
                jugador1.Usuario.Jugando = false;
                eventoFinalizarPartida?.Invoke(this, EventArgs.Empty);
                eventoCargarDatosUsuario?.Invoke(jugador1.Usuario, EventArgs.Empty);
                Partida partida = new(0, jugador1.ToString(), jugador2.ToString(), $"{jugador1.Puntaje} - {jugador2.Puntaje}", fecha, manosJugadas);
                eventoCargarDatosPartidas?.Invoke(partida, EventArgs.Empty);
            }
            if (jugador2.Puntaje == 15)
            {
                jugador2.Usuario.Victorias++;
                jugador1.Usuario.Derrotas++;
                InvocarEventoPartida($"\n\nTermino la partida \n\n--GANADOR: {jugador2.ToString()} \n\n Pts {jugador2.ToString()}: {jugador2.Puntaje} " +
                $"-- Pts {jugador1.Usuario.NickName}: {jugador1.Puntaje} \n\n");
                jugador2.Usuario.Jugando = false;
                eventoFinalizarPartida?.Invoke(this, EventArgs.Empty);
                Partida partida = new(0, jugador2.ToString(), jugador1.ToString(), $"{jugador2.Puntaje} - {jugador1.Puntaje}", fecha, manosJugadas);
                eventoCargarDatosPartidas?.Invoke(partida, EventArgs.Empty);
                eventoCargarDatosUsuario?.Invoke(jugador2.Usuario, EventArgs.Empty);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int calcularTrucoGanado()
        {
            switch (NivelTruco)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 4;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int calcularTrucoPerdido()
        {
            switch (NivelTruco - 1)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 4;
            }
            return 0;
        }



        /// <summary>
        /// Deserializa el mazo del archivo correspondiente, si no existe dicho archivo utiliza la funcion CargarMazo() y serializa el mazo.
        /// </summary>
        /// <returns></returns>
        public List<Carta> GenerarMazo()
        {
            List<Carta> nuevoMazo;

            SerializadorJSON<List<Carta>> jsonSerializer = new SerializadorJSON<List<Carta>>("MazoTruco");
            nuevoMazo = jsonSerializer.DeSerialize();

            if (nuevoMazo == null)
            {
                nuevoMazo = new List<Carta>();
                CargarMazo();
                nuevoMazo = mazo;
                jsonSerializer.Serialize(nuevoMazo);
            }
            return nuevoMazo;
        }


        /// <summary>
        /// Mezcla aleatoriamente las cartas del mazo
        /// </summary>
        public void MezclarMazo()
        {
            int indice;
            List<int> numeros = new List<int>();
            Random r = new Random();
            List<Carta> mazoTemp = new List<Carta>();
            while (numeros.Count < 40)
            {
                indice = r.Next(0, 40);
                if (!numeros.Contains(indice)) numeros.Add(indice);
            }
            foreach (int n in numeros)
            {
                mazoTemp.Add(mazo[n]);
            }
            mazo.Clear();
            mazo = mazoTemp;
        }


        /// <summary>
        /// Reparte 3 cartas a cada jugador en orden
        /// </summary>
        public void Repartir()
        {
            if (this.jugador1.EsMano)
            {

                jugador1.Mano.Add(mazo[0]);
                jugador2.Mano.Add(mazo[1]);
                jugador1.Mano.Add(mazo[2]);
                jugador2.Mano.Add(mazo[3]);
                jugador1.Mano.Add(mazo[4]);
                jugador2.Mano.Add(mazo[5]);
            }
            else
            {
                jugador2.Mano.Add(mazo[0]);
                jugador1.Mano.Add(mazo[1]);
                jugador2.Mano.Add(mazo[2]);
                jugador1.Mano.Add(mazo[3]);
                jugador2.Mano.Add(mazo[4]);
                jugador1.Mano.Add(mazo[5]);
            }            
        }

        /// <summary>
        /// Carga el mazo con las cartas
        /// </summary>
        public void CargarMazo()
        {
            Mazo.Add(new Carta(1, "espada"));
            Mazo.Add(new Carta(2, "espada"));
            Mazo.Add(new Carta(3, "espada"));
            Mazo.Add(new Carta(4, "espada"));
            Mazo.Add(new Carta(5, "espada"));
            Mazo.Add(new Carta(6, "espada"));
            Mazo.Add(new Carta(7, "espada"));
            Mazo.Add(new Carta(10, "espada"));
            Mazo.Add(new Carta(11, "espada"));
            Mazo.Add(new Carta(12, "espada"));
            Mazo.Add(new Carta(1, "basto"));
            Mazo.Add(new Carta(2, "basto"));
            Mazo.Add(new Carta(3, "basto"));
            Mazo.Add(new Carta(4, "basto"));
            Mazo.Add(new Carta(5, "basto"));
            Mazo.Add(new Carta(6, "basto"));
            Mazo.Add(new Carta(7, "basto"));
            Mazo.Add(new Carta(10, "basto"));
            Mazo.Add(new Carta(11, "basto"));
            Mazo.Add(new Carta(12, "basto"));
            Mazo.Add(new Carta(1, "oro"));
            Mazo.Add(new Carta(2, "oro"));
            Mazo.Add(new Carta(3, "oro"));
            Mazo.Add(new Carta(4, "oro"));
            Mazo.Add(new Carta(5, "oro"));
            Mazo.Add(new Carta(6, "oro"));
            Mazo.Add(new Carta(7, "oro"));
            Mazo.Add(new Carta(10, "oro"));
            Mazo.Add(new Carta(11, "oro"));
            Mazo.Add(new Carta(12, "oro"));
            Mazo.Add(new Carta(1, "copa"));
            Mazo.Add(new Carta(2, "copa"));
            Mazo.Add(new Carta(3, "copa"));
            Mazo.Add(new Carta(4, "copa"));
            Mazo.Add(new Carta(5, "copa"));
            Mazo.Add(new Carta(6, "copa"));
            Mazo.Add(new Carta(7, "copa"));
            Mazo.Add(new Carta(10, "copa"));
            Mazo.Add(new Carta(11, "copa"));
            Mazo.Add(new Carta(12, "copa"));
        }

        private int JugadorContestaEnvido(Jugador jContesta, Jugador jCanta)
        {
            int decision;

            decision = jContesta.DecidirEnvido(envidosCantados[envidosCantados.Count - 1], jCanta);

            if (decision == envidosCantados[envidosCantados.Count - 1]) //Si acepta
            {
                if (jCanta.EnvidoJugadorCantado > jContesta.EnvidoJugadorCantado) //Si gana jCanta
                {
                    jCanta.Puntaje += calcularEnvidoGanado(jContesta.Puntaje);
                }
                if (jCanta.EnvidoJugadorCantado < jContesta.EnvidoJugadorCantado) // Si gana jContesta
                {
                    jContesta.Puntaje += calcularEnvidoGanado(jCanta.Puntaje);
                }
                if (jCanta.EnvidoJugadorCantado == jContesta.EnvidoJugadorCantado) // Si empatan 
                {
                    if (jCanta.EsMano == true) //Si es mano Canta, gana el
                    {
                        jCanta.Puntaje += calcularEnvidoGanado(jContesta.Puntaje);
                    }
                    else // SI es mano Contesta, gana el
                    {
                        jContesta.Puntaje += calcularEnvidoGanado(jCanta.Puntaje);
                    }
                }
            }
            //else if (decision > envidosCantados[envidosCantados.Count - 1]) decision = envidosCantados[envidosCantados.Count - 1];
            return decision;
        }

        //private Jugador GanadorMano()
        //{
        //    int contadorj1 = 0;
        //    int contadorj2 = 0;

        //    foreach (Jugador j in ganador)
        //    {              
        //        if (j == jugador1) contadorj1++;
        //        if (j == jugador2) contadorj2++;
        //        if (contadorj1 == 2 - parda) {  return jugador1; }
        //        if (contadorj2 == 2 - parda) {  return jugador2; }
        //    }
        //    return null;
        //}


        //private void TrucoMano(Jugador jMano, Jugador jNoMano)
        //{
        //    int decision;
        //    InvocarEventoPartida($" {jMano.ToString()}: Canto Truco! \n\n");
        //    trucosCantados.Add(nivelTruco); //Canta --> JMANO            
        //    Thread.Sleep(700);
        //    nivelTruco = jNoMano.DecidirTruco(nivelTruco, jMano);
        //    if (nivelTruco == 0)
        //    {              
        //        InvocarEventoPartida($"{jNoMano.ToString()} Contesto: No Quiero! \n\n");

        //        jMano.Puntaje += calcularTrucoGanado();
        //        InvocarEventoPartida($"{jMano.ToString()} gano: {calcularTrucoGanado()} pts \n\n");
        //        InvocarEventoPartida($"");
        //        trucoFinalizado = true;
        //    }
        //    else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //    {
        //        InvocarEventoPartida($"{jNoMano.ToString()} Contesto: Quiero! \n\n");
        //        Thread.Sleep(700);
        //    }
        //    else if (nivelTruco > trucosCantados[trucosCantados.Count - 1]) //Retruca
        //    {   
        //        InvocarEventoPartida($"{jNoMano.ToString()} Contesto: Quiero ReTruco! \n\n");
        //        Thread.Sleep(700);
        //        trucosCantados.Add(nivelEnvido); // RETRUCA JNOMANO
        //        nivelTruco = jMano.DecidirTruco(nivelEnvido, jNoMano); // CONTESTA JMANO
        //        if (nivelTruco == 0)
        //        {
        //            InvocarEventoPartida($"{jMano.ToString()} Contesto: No Quiero! \n\n");
        //            jNoMano.Puntaje += calcularTrucoGanado();
        //            InvocarEventoPartida($"{jNoMano.ToString()} gano: {calcularTrucoGanado()} pts");
        //            trucoFinalizado = true;
        //        }
        //        else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //        {
        //            InvocarEventoPartida($"{jMano.ToString()} Contesto: Quiero! \n\n");
        //            Thread.Sleep(700);
        //        }
        //        else if (nivelTruco > trucosCantados[trucosCantados.Count - 1]) //Retruca
        //        {
        //            InvocarEventoPartida($"{jMano.ToString()} Contesto: Quiero Vale Cuatro!! \n\n");
        //            Thread.Sleep(700);
        //            trucosCantados.Add(nivelEnvido); // RETRUCA J MANO
        //            nivelTruco = jNoMano.DecidirTruco(nivelEnvido, jMano);//CONTESTA JNOMANO
        //            if (nivelTruco == 0)
        //            {
        //                InvocarEventoPartida($"{jNoMano.ToString()} Contesto: No Quiero! \n\n");
        //                jMano.Puntaje += calcularTrucoGanado();
        //                InvocarEventoPartida($"{jMano.ToString()} gano: {calcularTrucoGanado()} pts \n\n");
        //                trucoFinalizado = true;
        //            }
        //            else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //            {    
        //                InvocarEventoPartida($"{jNoMano.ToString()} Contesto: Quiero! \n\n");
        //                Thread.Sleep(700);
        //            }
        //        }

        //    }
        //}

        //private void TrucoNoMano(Jugador jMano, Jugador jNoMano)
        //{

        //    if (nivelTruco == 1)
        //    {
        //        InvocarEventoPartida($" {jNoMano.ToString()}: Canto Truco! \n\n");               
        //    }

        //    trucosCantados.Add(nivelTruco);
        //    Thread.Sleep(700);
        //    nivelTruco = jMano.DecidirTruco(nivelTruco, jNoMano);
        //    if (nivelTruco == 0)
        //    {
        //        InvocarEventoPartida($"{jMano.ToString()} Contesto: No Quiero! \n\n");
        //        jNoMano.Puntaje += calcularTrucoGanado();
        //        InvocarEventoPartida($"{jNoMano.ToString()} gano: {calcularTrucoGanado()} pts \n\n");
        //        trucoFinalizado = true;
        //    }
        //    else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //    {
        //        InvocarEventoPartida($"{jMano.ToString()} Contesto: Quiero! \n\n");
        //        Thread.Sleep(700);
        //    }
        //    else if (nivelTruco > trucosCantados[trucosCantados.Count - 1]) //Retruca
        //    {
        //        InvocarEventoPartida($"{jMano.ToString()} Contesto: Quiero ReTruco! \n\n");
        //        Thread.Sleep(700);
        //        trucosCantados.Add(nivelEnvido); // RETRUCA JNOMANO
        //        nivelTruco = jNoMano.DecidirTruco(nivelEnvido, jMano); // CONTESTA JMANO
        //        if (nivelTruco == 0)
        //        {
        //            InvocarEventoPartida($"{jNoMano.ToString()} Contesto: No Quiero! \n\n");
        //            jMano.Puntaje += calcularTrucoGanado();
        //            InvocarEventoPartida($"{jMano.ToString()} gano: {calcularTrucoGanado()} pts \n\n");
        //            trucoFinalizado = true;
        //        }
        //        else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //        {
        //            InvocarEventoPartida($"{jNoMano.ToString()} Contesto: Quiero! \n\n");
        //            Thread.Sleep(700);
        //        }
        //        else if (nivelTruco > trucosCantados[trucosCantados.Count - 1]) //Retruca
        //        {
        //            InvocarEventoPartida($"{jNoMano.ToString()} Contesto: Quiero Vale Cuatro!! \n\n");
        //            Thread.Sleep(700);
        //            trucosCantados.Add(nivelEnvido); // RETRUCA J MANO
        //            nivelTruco = jMano.DecidirTruco(nivelEnvido, jNoMano);//CONTESTA JNOMANO
        //            if (nivelTruco == 0)
        //            {
        //                InvocarEventoPartida($"{jMano.ToString()} Contesto: No Quiero! \n\n");
        //                jNoMano.Puntaje += calcularTrucoGanado();
        //                InvocarEventoPartida($"{jNoMano.ToString()} gano: {calcularTrucoGanado()} pts \n\n");
        //                trucoFinalizado = true;
        //            }
        //            else if (nivelTruco == trucosCantados[trucosCantados.Count - 1]) //Acepta
        //            {
        //                InvocarEventoPartida($"{jMano.ToString()} Contesto: Quiero! \n\n");
        //                Thread.Sleep(700);
        //            }
        //        }

        //    }
        //}

        //private void JugarMano()
        //{
        //        mano.CantCartasBuenas = mano.CalcularCartasBuenas();
        //        pie.CantCartasBuenas = pie.CalcularCartasBuenas();
        //        InvocarEventoPartida($"MANO CARTAS{mano.CantCartasBuenas} -- PÏE CARTAS { pie.CantCartasBuenas} ");

        //        Jugador temp = JugarRonda(mano, pie, 1); //PRIMERA           
        //        ganador.Add(temp);
        //        //Primera Ronda
        //        if(parda == 0)InvocarEventoPartida($"Ganador Ronda: {ganador[0].Usuario.NickName} con la carta {ganador[0].CartasJugadas[0].ToString()} \n\n"); //Muestro carta                                                                                                                                               
        //        if (mano != ganador[0]) { Jugador aux = mano; mano = pie; pie = aux; }

        //        //Segunda Ronda
        //        //CHEQUEO SI HUBO PARDA EN LA PRIMERA CON EL CONTADOR Y PRENDO UNA BANDERA
        //        if (parda == 1) pardaLaMejor = true;
        //        temp = JugarRonda(mano, pie, 2);
        //        if (temp is not null)
        //        {
        //            ganador.Add(temp); //SEGUNDA                    
        //            InvocarEventoPartida($"Ganador Ronda: {ganador[1].Usuario.NickName} con la carta {ganador[1].CartasJugadas[1].ToString()} \n\n");
        //            Thread.Sleep(500);

        //            if (pardaLaMejor)
        //            {
        //                InvocarEventoPartida($"Ganador Mano: {ganador[1].Usuario.NickName} con la carta {ganador[1].CartasJugadas[1].ToString()} \n\n");
        //                Thread.Sleep(500);
        //                ganador.Add(temp);//DEFINE EL GANADOR
        //            }
        //            //SI LA BANDERA == TRUE ENTONCES GANA EL GANADOR DE LA RONDA 2

        //            if (GanadorMano() == null)
        //            {
        //                if (mano != ganador[1]) { Jugador aux = mano; mano = pie; pie = aux; }
        //                temp = JugarRonda(mano, pie, 3); //TERCER RONDA
        //                if (temp is not null)
        //                {
        //                    ganador.Add(temp); //tercera Ronda
        //                    InvocarEventoPartida($"Ganador Ronda: {ganador[2].Usuario.NickName} con la carta {ganador[2].CartasJugadas[2].ToString()} \n\n");
        //                    Thread.Sleep(500);
        //                    if (GanadorMano() == Jugador1)
        //                    {
        //                        Jugador1.Puntaje += calcularTrucoGanado();
        //                        InvocarEventoPartida($"Ganador Mano: {jugador1.ToString()} \n\n");
        //                        Thread.Sleep(500);
        //                        InvocarEventoPartida($"{jugador1.ToString()} gano: {calcularTrucoGanado()}pts \n\n");
        //                        Thread.Sleep(500);
        //                    }
        //                    else if (GanadorMano() == Jugador2)
        //                    {
        //                        Jugador2.Puntaje += calcularTrucoGanado();
        //                        InvocarEventoPartida($"Ganador Mano: {jugador2.Usuario.NickName} \n\n");
        //                        Thread.Sleep(500);
        //                        InvocarEventoPartida($"{jugador2.ToString()} gano: {calcularTrucoGanado()}pts \n\n");
        //                        Thread.Sleep(500);
        //                    }
        //                }
        //                //

        //            }
        //        else
        //        {
        //            if (jugador1 == ganadorManoTruco) { ganador.Add(jugador1); ganador.Add(jugador1); }
        //            if (jugador2 == ganadorManoTruco) { ganador.Add(jugador2); ganador.Add(jugador2); }

        //            if (GanadorMano() == Jugador1)
        //            {
        //                Jugador1.Puntaje += calcularTrucoGanado();
        //                InvocarEventoPartida($"Ganador Mano: {jugador1.ToString()} \n\n");
        //                Thread.Sleep(500);
        //                InvocarEventoPartida($"{jugador1.ToString()} gano: {calcularTrucoGanado()}pts \n\n");
        //                Thread.Sleep(500);
        //            }                    
        //            if (GanadorMano() == Jugador2)
        //            {
        //                Jugador2.Puntaje += calcularTrucoGanado();
        //                InvocarEventoPartida($"Ganador Mano: {jugador2.Usuario.NickName} \n\n");
        //                Thread.Sleep(500);
        //                InvocarEventoPartida($"{jugador2.ToString()} gano: {calcularTrucoGanado()}pts \n\n");
        //                Thread.Sleep(500);
        //            }
        //           
        //        }
        //    }

        //    manosJugadas++;
        //}
    }
}
