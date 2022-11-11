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
    public partial class DetalleJugador : Form
    {
        private int derrotas;
        private int victorias;
        private string nick;
        private int id;

        public int Derrotas { get => derrotas; set => derrotas = value; }
        public int Victorias { get => victorias; set => victorias = value; }
        public string Nick { get => nick; set => nick = value; }
        public int Id { get => id; set => id = value; }

        public DetalleJugador()
        {
            InitializeComponent();
        }

        public DetalleJugador(Usuario usuario) : this()
        {
            txtNick.Text = usuario.NickName;
            txtVictorias.Text = usuario.Victorias.ToString();
            txtDerrotas.Text = usuario.Derrotas.ToString();
            this.id = usuario.Id;
        }

        private void DetalleJugador_Load(object sender, EventArgs e)
        {

        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            bool todoOk = true;
            foreach(var item in this.Controls)
            {
                if(item is TextBox)
                {
                    TextBox t = item as TextBox;
                    if(t.Text == "")
                    {
                         errorProvider1.SetError(t, "Rellene todos los campos para confirmar");
                        todoOk = false;
                    }
                   
                }
            }
            if (todoOk)
            {
                this.nick = txtNick.Text;
                this.victorias = int.Parse(txtVictorias.Text);
                this.derrotas = int.Parse(txtDerrotas.Text);                
                this.DialogResult = DialogResult.OK;
            }          
        }

        private void txtVictorias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !(e.KeyChar == '\b')) e.Handled = true;
        }

        private void txtDerrotas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !(e.KeyChar == '\b')) e.Handled = true;
        }
    }
}
