using _2doParcial;
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
    public partial class FormSala : Form
    {
        private Sala sala;
        private CancellationTokenSource cancelarToken = new(); 

        public FormSala(Sala sala)
        {
            
            InitializeComponent();
            //richTextBox1.Text = sala.Log;
            if (sala is null)
            {
                MessageBox.Show("Error al crear la sala, intente de nuevo");
                this.Close();
            }
            else
            {
                this.sala = sala;
                sala.actualizar += ActualizarRichText;                
            }
        }

        public void ActualizarRichText(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = ActualizarRichText;
                Invoke(delegado,sender, e);
            }
            else
            {
                label1.Text = $" Jugador 1:{sala.Jugador1.ToString()} -- Puntaje: {sala.Jugador1.Puntaje}";
                label2.Text = $" Jugador 2:{sala.Jugador2.ToString()} -- Puntaje: {sala.Jugador2.Puntaje}";
                richTextBox1.Text = richTextBox1.Text.Insert(0, sala.Log);
            }          
        }    

        private void FormSala_FormClosing(object sender, FormClosingEventArgs e)
        {
            sala.actualizar -= ActualizarRichText;
        }

        private void FormSala_Load(object sender, EventArgs e)
        {
            ActualizarRichText(sender, e);
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea finalizar la partida?", "Finalizacion partida", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                MessageBox.Show("Se cancelo la partida");                
                sala.TerminoPartida = true;
                this.Hide();
                this.cancelarToken.Cancel();
            }
            else
            {
                MessageBox.Show("Se cancelo el cierre la partida");
            }
        }
    }
}
