using _2doParcial;
using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Truco
{
    public partial class Lobby : Form
    {
        private List<Sala> listaSalas;
        private List<Usuario> listaUsuarios;
        public List<Sala> ListaSalas { get => listaSalas; set => listaSalas = value; }
        public List<Usuario> ListaUsuarios { get => listaUsuarios; set => listaUsuarios = value; }

        public Lobby()
        {
            InitializeComponent();
            ListaSalas = new();            
            this.ActualizarListaUsuarios();
        }

        private void CargarDatosPartidas(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = CargarDatosPartidas;
                Invoke(delegado, sender, e);
            }
            else
            {
                try
                {
                    Partida partida = new();
                    partida.Agregar((Partida)sender);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al cargar los datos de la partida en la base de datos.", "Error");
                    Thread.Sleep(1000);
                }
            }
           
        }



        private void ActualizarDatosUsuarios(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = ActualizarDatosUsuarios;
                Invoke(delegado, sender, e);
            }
            else
            {
                try
                {
                    Usuario usuario = new();
                    usuario.Modificar((Usuario)sender);
                    ActualizarListaUsuarios();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al actualizar los datos del usuario en la base de datos. ", "Error");
                    Thread.Sleep(1000);
                }
            }            
        }

        private void actualizarList(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = actualizarList;
                Invoke(delegado, sender, e);
            }
            else
            {
                listBox1.Items.Clear();                
                foreach (Sala s in ListaSalas)
                {
                    if (!s.TerminoPartida)
                    {
                        listBox1.Items.Add(s);
                    }
                }
            }           
        }

        private void ActualizarListTerminadas(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = ActualizarListTerminadas;
                Invoke(delegado, sender, e);
            }
            else
            {
                listBox2.Items.Clear();
                foreach (Sala s in ListaSalas)
                {
                    if (s.TerminoPartida)
                    {
                        listBox2.Items.Add(s);
                    }
                }
            }
        }

        private void ActualizarListaEventos(object sender, EventArgs e)
        {
            ActualizarListaUsuarios();
        }

        private void ActualizarListaUsuarios() 
        {
            try
            {
                Usuario usuario = new();
                this.ListaUsuarios = usuario.Obtener();    
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar con la base de datos. El programa se cerrara", "Error");
                Thread.Sleep(1000);
                Application.Exit();
            }
        }




        private void btnCrear_Click(object sender, EventArgs e)
        {

            List<Usuario> listaUsuariosDisponibles = new();
           

            if (listaSalas.Count == 0) { listaUsuariosDisponibles = listaUsuarios; }
            else
            {
                listaUsuariosDisponibles = listaUsuarios;
                foreach (Sala s in listaSalas)
                {
                    if (s.Jugador1.Usuario.Jugando == true && s.Jugador1.Usuario.Jugando == true)
                    {
                        listaUsuariosDisponibles.Remove(s.Jugador1.Usuario);
                        listaUsuariosDisponibles.Remove(s.Jugador2.Usuario);
                    }
                }
            }
            CrearSala salaNueva = new CrearSala(listaUsuariosDisponibles);
            salaNueva.ShowDialog();
            if (salaNueva.DialogResult == DialogResult.OK)
            {
                Sala sala = new(new Jugador(salaNueva.Jugador11), new Jugador(salaNueva.Jugador21));
                sala.eventoFinalizarPartida += actualizarList;
                sala.eventoFinalizarPartida += ActualizarListTerminadas;
                sala.eventoCargarDatosPartidas += CargarDatosPartidas;
                sala.eventoCargarDatosUsuario += ActualizarDatosUsuarios;
                ListaSalas.Add(sala);
                actualizarList(sala, EventArgs.Empty);
            }
            else
            {
                salaNueva.Close();
            }
        }


        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem is null)
                MessageBox.Show("Error al seleccionar la sala, intentelo de nuevo.");
            else
            {
                FormSala form = new((Sala)listBox2.SelectedItem);
                form.Show();
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem is null)
                MessageBox.Show("Error al seleccionar la sala, intentelo de nuevo.");
            else
            {
                Sala sala = (Sala)listBox1.SelectedItem;
                FormSala form = new(sala);
                form.Show();
            }
        }

        private void btnJugadores_Click(object sender, EventArgs e)
        {
            ActualizarListaUsuarios();
            Jugadores formJugadores = new(ListaUsuarios);
            formJugadores.ShowDialog();
        }

        private void Lobby_Load(object sender, EventArgs e)
        {
            ActualizarListaUsuarios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmPartidas formPartidas = new();
            formPartidas.ShowDialog();
        }

 
    }
}
