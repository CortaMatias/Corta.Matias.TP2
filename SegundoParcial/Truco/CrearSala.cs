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
    public partial class CrearSala : Form
    {
        private Usuario jugador1;
        private Usuario jugador2;
        private List<Usuario> listaUsuarios;
        private List<Usuario> listaUsuariosAux;

        

        public CrearSala(List<Usuario> listaUsuarios) 
        {                  
            this.listaUsuarios = listaUsuarios;
            InitializeComponent();
        }
        public Usuario Jugador11 { get => jugador1; set => jugador1 = value; }
        public Usuario Jugador21 { get => jugador2; set => jugador2 = value; }

        private void button1_Click(object sender, EventArgs e)
        {
           if(cmbJugador1.SelectedItem is not null)
            {
                if(cmbJugador2.SelectedItem is not null)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }            
        }

        private void CrearSala_Load(object sender, EventArgs e)
        {
            cmbJugador1.Items.Clear();
            cmbJugador2.Items.Clear();
            foreach (Usuario u in listaUsuarios)
                {
                    this.cmbJugador1.Items.Add($"Id:{u.Id} --  Nick : {u.NickName}");
                    this.cmbJugador2.Items.Add($"Id:{u.Id} --  Nick : {u.NickName}");
                }          
        }

        private void cmbJugador1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = cmbJugador1.SelectedIndex;
            try { cmbJugador2.Items.RemoveAt(indice); }
            catch (Exception){ }
                       
            this.jugador1 = listaUsuarios[indice];
            cmbJugador2.Enabled = true;
            listaUsuariosAux = listaUsuarios;
            listaUsuariosAux.RemoveAt(indice);
        }

        private void cmbJugador2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = cmbJugador2.SelectedIndex;         
            if(indice != -1) this.jugador2 = listaUsuariosAux[indice];

        }
    }
}
