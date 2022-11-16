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
    public partial class FormSala : Form
    {
        private Sala sala;
        private CancellationTokenSource cancelarToken = new();
        

        public FormSala(Sala sala)
        {     
            InitializeComponent();         
            this.sala = sala;
            sala.Token = cancelarToken.Token;
            sala.actualizar += ActualizarRichText;               
            
        }

        public void ActualizarRichText(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                EventHandler delegado = ActualizarRichText;
                try
                {
                    Invoke(delegado, sender, e);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                label1.Text = $"{sala.Jugador1.ToString()}--Puntos:{sala.Jugador1.Puntaje}";
                label2.Text = $"{sala.Jugador2.ToString()}--Puntos:{sala.Jugador2.Puntaje}";
                richTextBox1.Text = sala.Log;               
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
                MessageBox.Show("Se cancelo la partida, se agregara a la lista de partidas terminadas");
                this.cancelarToken.Cancel();
                sala.TerminoPartida = true;
                this.Hide();                
            }
            else
            {
                MessageBox.Show("Se cancelo el cierre la partida");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ManejadorArchivoTxt.Escribir(richTextBox1.Text, $"Partida{ManejadorArchivoTxt.Partidas}");
                MessageBox.Show("Se guardaron los logs de la partida en una carpeta sobre el Escritorio"); 
            }
            catch (Exception)
            {
                MessageBox.Show("Error al crear el archivo");
            }
        }
    }
}
