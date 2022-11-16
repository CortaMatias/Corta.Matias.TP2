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
    public partial class Jugadores : Form
    {

        List<Usuario> listaUsuarios;


        public Jugadores(List<Usuario> listaUsuarios)
        {
            this.listaUsuarios = listaUsuarios;
            InitializeComponent();
        }

        private void Jugadores_Load(object sender, EventArgs e)
        {             
            dgvJugadores.DataSource = listaUsuarios;
            dgvJugadores.Columns[4].Visible = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(dgvJugadores.CurrentRow.DataBoundItem is not null)
            {               
                try
                {
                    Usuario usuario =((Usuario)dgvJugadores.CurrentRow.DataBoundItem);
                    if (usuario.Eliminar(usuario))
                    {
                        this.listaUsuarios = usuario.Obtener();
                        dgvJugadores.DataSource = listaUsuarios;
                        dgvJugadores.Columns[4].Visible = false;
                    }
                }
                catch (Exception )
                {
                    MessageBox.Show("Error al eliminar el usuario");
                }
               
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dgvJugadores.CurrentRow.DataBoundItem is not null)
            {
                
               
                DetalleJugador formDetalle = new((Usuario) dgvJugadores.CurrentRow.DataBoundItem);
                formDetalle.txtNick.Enabled = false;
                formDetalle.ShowDialog();                
                try
                {
                    Usuario editado = new(formDetalle.Id, formDetalle.Nick, formDetalle.Victorias, formDetalle.Derrotas);
                    if (editado.Modificar(editado))
                    {
                        this.listaUsuarios = editado.Obtener();
                        dgvJugadores.DataSource = listaUsuarios;
                        dgvJugadores.Columns[4].Visible = false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al modificar el usuario");
                }
            }
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DetalleJugador formDetalle = new();
            formDetalle.txtNick.Enabled = true;
            formDetalle.ShowDialog();
           
            try
            {
                Usuario nuevoUsuario = new(0, formDetalle.Nick, formDetalle.Victorias, formDetalle.Derrotas);
                if (nuevoUsuario.Agregar(nuevoUsuario))
                {
                    this.listaUsuarios = nuevoUsuario.Obtener();
                    dgvJugadores.DataSource = listaUsuarios;
                    dgvJugadores.Columns[4].Visible = false;
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("Error al agregar pasajero");
            }
        }


    }
}
