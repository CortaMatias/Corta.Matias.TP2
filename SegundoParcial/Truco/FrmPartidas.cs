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
    public partial class FrmPartidas : Form
    {
        List<Partida> listaPartidas;

        public FrmPartidas()
        {
            InitializeComponent();
        }



        private void cargarDataGrid()
        {
            try
            {
                Partida partida = new();
                this.listaPartidas = partida.Obtener();
                this.dgvPartida.DataSource = listaPartidas;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar con la base de datos.", "Error");
                Thread.Sleep(1000);
                this.Hide();
            }
        }

        private void FrmPartidas_Load(object sender, EventArgs e)
        {
            cargarDataGrid();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPartida.CurrentRow.DataBoundItem is not null)
            {
                try
                {
                    Partida partida = ((Partida)dgvPartida.CurrentRow.DataBoundItem);
                    if (partida.Eliminar(partida)) 
                    {
                        this.listaPartidas = partida.Obtener();
                        dgvPartida.DataSource = listaPartidas;                       
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al eliminar la partida");
                }

            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarDataGrid();
        }
    }
}
