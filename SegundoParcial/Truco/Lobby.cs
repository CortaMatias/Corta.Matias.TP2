using _2doParcial;
using _2doParcial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Truco
{
    public partial class Lobby : Form
    {
        List<Sala> listaSalas;
        List<Usuario> listaUsuarios;

        public Lobby()
        {
            listaSalas = new();
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            this.ActualizarListaUsuarios();
            CrearSala salaNueva = new CrearSala(listaUsuarios);
            salaNueva.ShowDialog();

            if(salaNueva.DialogResult == DialogResult.OK)
            {
                Sala sala = new(new Jugador(salaNueva.Jugador11), new Jugador(salaNueva.Jugador21));
                sala.eventoFinalizarPartida += actualizarList;
                listaSalas.Add(sala);
                actualizarList(sala, EventArgs.Empty);
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
                foreach (Sala s in listaSalas)
                {
                    if (!s.TerminoPartida)
                    {
                        listBox1.Items.Add(s);
                    }
                }
            }           
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FormSala form = new((Sala)listBox1.SelectedItem);
            form.Show();
        }

        private void btnJugadores_Click(object sender, EventArgs e)
        {
            Jugadores formJugadores = new(listaUsuarios);
            formJugadores.ShowDialog();
        }

        private void Lobby_Load(object sender, EventArgs e)
        {
                       
        }

        private void ActualizarListaUsuarios() 
        {
            try
            {
                DataAccess_Usuarios peticion = new();
                listaUsuarios = peticion.ObtenerUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
