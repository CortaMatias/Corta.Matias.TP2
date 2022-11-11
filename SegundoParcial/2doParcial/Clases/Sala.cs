using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2doParcial
{
    public class Sala
    {
        private Jugador jugador1;
        private Jugador jugador2;
        private Jugador mano;
        private Jugador pie;
        private Jugador jGanadorEnvido;
        private Jugador jPerdedorEnvido;
        private Jugador ganadorPartida;
        private Jugador ganadorManoAnterior;

        private List<Carta> mazo;
        private List<Carta> mesa;
        private List<int> envidosCantados;
        private List<int> trucosCantados;
        private List<Jugador> ganador;

        private bool terminoPartida = false;
        private bool envidoFinalizado = false;
        private bool trucoFinalizado = false;

        private int id;
        private int nivelTruco = 0;
        private int nivelEnvido = 0;
        private int turnosJugados;
        private int manosJugadas;

        private string log;
        private Task tarea;



        private CancellationToken token;




        public event EventHandler actualizar;
        public event EventHandler eventoFinalizarPartida;

        public Jugador Jugador1 { get => jugador1; set => jugador1 = value; }
        public Jugador Jugador2 { get => jugador2; set => jugador2 = value; }
        public List<Carta> Mazo { get => mazo; set => mazo = value; }
        public string Log { get => log; set => log = value; }
        public Task Tarea { get => tarea; set => tarea = value; }
        public CancellationToken Token { get => token; set => token = value; }
        public bool TerminoPartida { get => terminoPartida; set => terminoPartida = value; }

        public Sala(Jugador j1, Jugador j2)
        {
            ganador = new();
            mesa = new();
            envidosCantados = new();
            this.jugador1 = j1;
            this.jugador2 = j2;
            this.mazo = new List<Carta>();
            trucosCantados = new();
            this.log = "";
            this.Tarea = new Task(() => ComenzarJuego());
            Tarea.Start();
            
        }

        private void ResetearStats()
        {
            jugador1.EnvidoJugadorCantado = -1;
            jugador2.EnvidoJugadorCantado = -1;
            mano = null;
            pie = null;
            nivelTruco = 0;
            nivelEnvido = 0;
            mesa.Clear();
            ganador.Clear();
            trucosCantados.Clear();
            envidosCantados.Clear();
            envidoFinalizado = false;
            trucoFinalizado = false;
            jugador1.Mano.Clear();
            jugador2.Mano.Clear();
            jugador1.CartasJugadas.Clear();
            jugador2.CartasJugadas.Clear();           
        }

        public void InvocarEventoPartida(string mensaje)
        {
            log = mensaje;
            actualizar?.Invoke(this, EventArgs.Empty);
        }

        public void TerminarPartida()
        {
            
        }


        public override string ToString()
        {
            return $"Jugador 1:{jugador1.ToString()} -- Jugador 2:{jugador2.ToString()}";
        }


        private Jugador JugarRonda(Jugador jMano, Jugador jNoMano, int turno)
        {
            Jugador ganador = jMano;
            jMano.EsMano = true;
            jNoMano.EsMano = false;
            // ======================== jMano ============================
            if (turno == 1 && !envidoFinalizado) EnvidoMano(jMano, jNoMano);
            

            mesa.Add(jMano.JugarTurno(jNoMano));
            InvocarEventoPartida($" {jMano.ToString()}: jugo {jMano.CartasJugadas[turno - 1].ToString()} \n\n");
            Thread.Sleep(500);
            // ======================== jMano ============================

            
            // ========= NO MANO =======================
            if (turno == 1 && !envidoFinalizado)            
                EnvidoNoMano(jMano, jNoMano);                   
                
            

            mesa.Add(jNoMano.JugarTurno(jMano));
            InvocarEventoPartida($" {jNoMano.ToString()} jugo: {jNoMano.CartasJugadas[turno - 1].ToString()} \n\n");
            Thread.Sleep(500);
            // ========= NO MANO =======================
         

            ganador = ganadorRonda(turno - 1, jMano, jNoMano);
            if (ganador is null)
                return jMano;
            turnosJugados++;

            return ganador;
        } 

        private void JugarMano()
        {
            Jugador temp = JugarRonda(mano, pie, 1);
            if (temp is null) //JUGAR RONDA TIENE QUE DEVOLVER NULL SI SE CANTO TRUCO Y NO SE QUISO Y CORTAMOS LA MANO
            {

            }
            else 
            {
                ganador.Add(temp);
            }
            //Primera Ronda
            InvocarEventoPartida( $"Ganador Ronda: {ganador[0].Usuario.NickName} con la carta {ganador[0].CartasJugadas[0].ToString()} \n\n"); //Muestro carta                                                                                                                                               
            if (mano != ganador[0]) { Jugador aux = mano; mano = pie; pie = aux; }
            



            //Segunda Ronda
            ganador.Add(JugarRonda(mano, pie, 2)); 
            InvocarEventoPartida($"Ganador Ronda: {ganador[1].Usuario.NickName} con la carta {ganador[1].CartasJugadas[1].ToString()} \n\n");
            Thread.Sleep(500);


            if (GanadorMano() == null)
            {
                if (mano != ganador[1]) { Jugador aux = mano; mano = pie; pie = aux; }
                ganador.Add(JugarRonda(mano, pie, 3)); //tercera Ronda
                InvocarEventoPartida($"Ganador Ronda: {ganador[2].Usuario.NickName} con la carta {ganador[2].CartasJugadas[2].ToString()} \n\n");                
                Thread.Sleep(500);
                if (GanadorMano() == null)
                    if (GanadorMano() == Jugador1)
                    {
                        ganadorManoAnterior = Jugador1;
                         Jugador1.EsMano = true; ///
                         Jugador2.EsMano = false;
                        InvocarEventoPartida($"Ganador Mano: {jugador1.Usuario.NickName} \n\n");
                        Thread.Sleep(500);
                    }
                    else if (GanadorMano() == Jugador2)
                    {
                        ganadorManoAnterior = Jugador2;
                        Jugador1.EsMano = false; ///
                        Jugador2.EsMano = true;
                        InvocarEventoPartida( $"Ganador Mano: {jugador2.Usuario.NickName} \n\n");
                        Thread.Sleep(500);
                    }
            }
            else if (GanadorMano() == Jugador1)
            {
                ganadorManoAnterior = Jugador1;
                Jugador1.EsMano = true; ///
                Jugador2.EsMano = false;
                InvocarEventoPartida( $"Ganador Mano: {jugador1.Usuario.NickName} \n\n");
                Thread.Sleep(500);
            }
            else if (GanadorMano() == Jugador2)
            {
                ganadorManoAnterior = Jugador2;
                Jugador1.EsMano = false; ///
                Jugador2.EsMano = true;
                InvocarEventoPartida($"Ganador Mano: {jugador2.Usuario.NickName} \n\n");                
            }
            if (jugador1.Puntaje >= 15 || jugador2.Puntaje >= 15)
            {
                TerminoPartida = true;
            }
            manosJugadas++;
        }

        private void ComenzarJuego()
        {            

            Thread.Sleep(3500);
            InvocarEventoPartida(" Iniciando Juego ...\n \n");
            Thread.Sleep(500);

            while (!TerminoPartida && !token.IsCancellationRequested)
            {
                Thread.Sleep(2700);
                InvocarEventoPartida($" ===== MANO {manosJugadas + 1} ===== \n\n");
                Thread.Sleep(700);
                InvocarEventoPartida(" Mezclando . . . \n\n");
                Thread.Sleep(700);

                Thread.Sleep(700);
                InvocarEventoPartida(" Repartiendo ...\n  \n");
                Thread.Sleep(700);
                ResetearStats();

                mazo = GenerarMazo();
                MezclarMazo();
                Repartir();
                jugador1.ordenarMano();
                jugador2.ordenarMano();
                //if (new Random().Next(1, 3) == 1) jugador1.EsMano = true;
                //else jugador2.EsMano = true;
                if (jugador1.EsMano) { mano = jugador1; pie = jugador2; }
                else if (jugador2.EsMano) { mano = jugador2; pie = jugador1; }

                if (mano == null) { mano = Jugador1; pie = Jugador2; }
                if (mano != ganadorManoAnterior) { Jugador aux = mano; mano = pie; pie = aux; }                
                InvocarEventoPartida($"\n ** Cartas {mano.ToString()}: {mano.MostrarCartas()} **\n\n");
                Thread.Sleep(700);
                InvocarEventoPartida($"\n ** Cartas {pie.ToString()}: {pie.MostrarCartas()} ** \n\n");
                Thread.Sleep(700);
                JugarMano();
            }
            if (token.IsCancellationRequested)
            {

            }            
            if (TerminoPartida)
            {
                if (jugador1.Puntaje > 15) jugador1.Puntaje = 15;
                if (jugador2.Puntaje > 15) Jugador2.Puntaje = 15;
                if (jugador1.Puntaje == 15 || jugador2.Puntaje == 15) 
                {
                    eventoFinalizarPartida?.Invoke(this, EventArgs.Empty);
                }
                InvocarEventoPartida($"\n\nTermino la partida --  Pts {jugador1.ToString()}: {jugador1.Puntaje} " +
                    $"-- Pts {jugador2.Usuario.NickName}: {jugador2.Puntaje} \n\n");
            }
            InvocarEventoPartida("");

        }


        private void EnvidoNoMano(Jugador jMano, Jugador jNoMano)
        {
            nivelEnvido = jNoMano.DecidirEnvido(nivelEnvido, jMano); //Decide no mano
            if (nivelEnvido != 0 && !envidoFinalizado)
            {

                envidosCantados.Add(nivelEnvido); //Canta
                InvocarEventoPartida($"{jNoMano.ToString()} Canto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                Thread.Sleep(500);
                nivelEnvido = jMano.DecidirEnvido(nivelEnvido, jNoMano); //Contesta

                if (nivelEnvido == 0) //No quiere
                {
                    jNoMano.Puntaje++;
                    InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($" ***{jNoMano.ToString()} Sumo 1 punto. *** \n\n");
                    Thread.Sleep(700);
                    envidoFinalizado = true;
                }
                if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                {
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(700);
                    InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                    Thread.Sleep(700);
                    jGanadorEnvido.Puntaje = calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                    envidoFinalizado = true;
                }
                if (nivelEnvido > envidosCantados[envidosCantados.Count - 1]) //Retruca
                {
                    InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                    Thread.Sleep(700);
                    envidosCantados.Add(nivelEnvido);
                    nivelEnvido = jNoMano.DecidirEnvido(nivelEnvido, jMano);
                    if (nivelEnvido == 0) //No quiere
                    {
                        jMano.Puntaje += calcularEnvidoPerdido();
                        InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                        Thread.Sleep(700);
                    }
                    if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\\n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(700);
                        InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                        Thread.Sleep(700);
                        jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                        InvocarEventoPartida("\n");
                        Thread.Sleep(700);
                    }
                    if (nivelEnvido > envidosCantados[envidosCantados.Count - 1])
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                        Thread.Sleep(700);
                        envidosCantados.Add(nivelEnvido);
                        nivelEnvido = jMano.DecidirEnvido(nivelEnvido, jNoMano);
                        if (nivelEnvido == 0)
                        {
                            jNoMano.Puntaje += calcularEnvidoGanado(jMano.Puntaje);
                            InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoGanado(jNoMano.Puntaje)} *** \n\n");
                            Thread.Sleep(700);
                        }
                        if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                        {
                            InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(700);
                            InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                            Thread.Sleep(700);
                            jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                            InvocarEventoPartida("\n");
                            Thread.Sleep(700);
                        }
                        if (nivelEnvido > envidosCantados[envidosCantados.Count - 1])
                        {
                            InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                            Thread.Sleep(700);
                            envidosCantados.Add(nivelEnvido);
                            nivelEnvido = jNoMano.DecidirEnvido(nivelEnvido, jNoMano);
                            if (nivelEnvido == 0)
                            {
                                jMano.Puntaje += calcularEnvidoPerdido();
                                InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                                Thread.Sleep(700);
                            }
                            if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                            {
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(700);
                                InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                                Thread.Sleep(700);
                                jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                                InvocarEventoPartida("");
                                Thread.Sleep(700);
                            }
                        }

                    }
                }
            }
        }

        private void EnvidoMano(Jugador jMano, Jugador jNoMano)
        {

            jMano.EnvidoJugadorCantado = jMano.calcularEnvido();
            jNoMano.EnvidoJugadorCantado = jNoMano.calcularEnvido();
            nivelEnvido = jMano.DecidirEnvido(nivelEnvido, jNoMano); //Decide  mano
            if (nivelEnvido != 0 && !envidoFinalizado)
            {
                envidosCantados.Add(nivelEnvido); //Canta
                InvocarEventoPartida($"{jMano.ToString()} Canto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                Thread.Sleep(500);
                nivelEnvido = jNoMano.DecidirEnvido(nivelEnvido, jMano); //Contesta

                if (nivelEnvido == 0) //No quiere
                {
                    jMano.Puntaje++;
                    InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($" ***{jMano.ToString()} Sumo 1 punto. *** \n\n");
                    Thread.Sleep(500);
                    envidoFinalizado = true;
                }
                if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                {
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                    Thread.Sleep(500);
                    InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                    Thread.Sleep(500);
                    jGanadorEnvido.Puntaje = calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                    envidoFinalizado = true;
                }
                if (nivelEnvido > envidosCantados[envidosCantados.Count - 1]) //Retruca 1
                {
                    InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                    Thread.Sleep(500);
                    envidosCantados.Add(nivelEnvido);
                    nivelEnvido = jMano.DecidirEnvido(nivelEnvido, jNoMano);
                    if (nivelEnvido == 0) //No quiere
                    {
                        jNoMano.Puntaje += calcularEnvidoPerdido();
                        InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                        Thread.Sleep(500);
                    }
                    if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                    {
                        InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                        Thread.Sleep(500);
                        InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                        Thread.Sleep(500);
                        jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                        InvocarEventoPartida("\n");
                        Thread.Sleep(500);
                    }
                    if (nivelEnvido > envidosCantados[envidosCantados.Count - 1])//--> Retruca 2
                    {
                        InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                        Thread.Sleep(500);
                        envidosCantados.Add(nivelEnvido);
                        nivelEnvido = jNoMano.DecidirEnvido(nivelEnvido, jMano);
                        if (nivelEnvido == 0)
                        {
                            jMano.Puntaje += calcularEnvidoPerdido();
                            InvocarEventoPartida($"{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($" ***{jMano.ToString()} sumo: {calcularEnvidoPerdido()} *** \n\n");
                            Thread.Sleep(500);
                        }
                        if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                        {
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                            Thread.Sleep(500);
                            InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                            Thread.Sleep(500);
                            jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                            InvocarEventoPartida("\n");
                            Thread.Sleep(500);
                        }
                        if (nivelEnvido > envidosCantados[envidosCantados.Count - 1]) //---> ReTruca 3
                        {
                            InvocarEventoPartida($"\n{jNoMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, true)}\n\n");
                            Thread.Sleep(500);
                            envidosCantados.Add(nivelEnvido);
                            nivelEnvido = jMano.DecidirEnvido(nivelEnvido, jNoMano);
                            if (nivelEnvido == 0)
                            {
                                jNoMano.Puntaje += calcularEnvidoGanado(jMano.Puntaje);
                                InvocarEventoPartida($"{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)} \n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($" ***{jNoMano.ToString()} sumo: {calcularEnvidoGanado(jNoMano.Puntaje)} *** \n\n");
                                Thread.Sleep(500);
                            }
                            if (nivelEnvido == envidosCantados[envidosCantados.Count - 1]) //Quiere
                            {
                                InvocarEventoPartida($"\n{jMano.ToString()} Contesto: {MensajeEnvido(nivelEnvido, false)}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($"\n{jNoMano.ToString()} Tantos:  {jNoMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida($"\n{jMano.ToString()} Tantos:  {jMano.EnvidoJugadorCantado}\n\n");
                                Thread.Sleep(500);
                                InvocarEventoPartida(ganadorEnvido(jMano, jNoMano));
                                Thread.Sleep(500);
                                jGanadorEnvido.Puntaje += calcularEnvidoGanado(jPerdedorEnvido.Puntaje);
                                InvocarEventoPartida("\n\n");
                                Thread.Sleep(500);
                            }
                        }

                    }
                }
            }
        }      

 




        private Jugador GanadorMano()
        {
            int contadorj1 = 0;
            int contadorj2 = 0;

            foreach (Jugador j in ganador)
            {
                if (j == jugador1) contadorj1++;
                if (j == jugador2) contadorj2++;
                if (contadorj1 == 2) { jugador1.Puntaje++; return jugador1; };
                if (contadorj2 == 2) { jugador2.Puntaje++; return jugador2; }
            }
            return null;
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


        private string ganadorEnvido(Jugador j1, Jugador j2)
        {
            StringBuilder sb = new();
            if (j1.EnvidoJugadorCantado > j2.EnvidoJugadorCantado)
            {
                sb.AppendLine($"Ganador Envido: {j1.ToString()} con {j1.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{j1.ToString()} Gano: {calcularEnvidoGanado(j2.Puntaje)}pts\n\n");
                jGanadorEnvido = j1;
                jPerdedorEnvido = j2;
            }
            else if (j1.EnvidoJugadorCantado < j2.EnvidoJugadorCantado)
            {

                sb.AppendLine($"Ganador Envido: {j2.ToString()} con {j2.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{j2.ToString()} Gano: {calcularEnvidoGanado(j1.Puntaje)}pts\n\n");
                jGanadorEnvido = j2;
                jPerdedorEnvido = j1;
            }
            else if (j1.EnvidoJugadorCantado == j2.EnvidoJugadorCantado)
            {
                sb.AppendLine($"Ganador Envido: {mano.ToString()} con {mano.EnvidoJugadorCantado} tantos \n\n");
                sb.AppendLine($"{mano.ToString()} Gano: {calcularEnvidoGanado(pie.Puntaje)}pts \n\n");
                jGanadorEnvido = mano;
                jPerdedorEnvido = pie;
            }
            return sb.ToString();
        }

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
            envidoFinalizado = true;
            return total;
        }

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

            envidoFinalizado = true;
            return total;
        }

        private Jugador ganadorRonda(int pos, Jugador jugador1, Jugador jugador2)
        {
            int rankingJugador1 = jugador1.CartasJugadas[pos].rankingCartas();
            int rankingJugador2 = jugador2.CartasJugadas[pos].rankingCartas();

            if (rankingJugador1 > rankingJugador2)
                return jugador1;
            if (rankingJugador1 < rankingJugador2)
                return jugador2;
            // Si empata
            return null;
        }

        private void CargarPuntaje(Jugador jugador, int puntos)
        {
            jugador.Puntaje += puntos;
        }




        private int calcularTrucoGanado()
        {
            switch (nivelTruco)
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

        private int calcularTrucoPerdido()
        {

            switch (nivelTruco - 1)
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

        private void MezclarMazo()
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

        private void Repartir()
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
    }
}
