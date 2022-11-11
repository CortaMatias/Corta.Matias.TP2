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
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(dgvJugadores.CurrentRow.DataBoundItem is not null)
            {
                DataAccess_Usuarios eliminar = new();
                try
                {
                    eliminar.BorrarUsuario((Usuario)dgvJugadores.CurrentRow.DataBoundItem);
                    this.listaUsuarios = eliminar.ObtenerUsuarios();
                    dgvJugadores.DataSource = listaUsuarios;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dgvJugadores.CurrentRow.DataBoundItem is not null)
            {
                DataAccess_Usuarios editar = new();
               
                DetalleJugador formDetalle = new((Usuario) dgvJugadores.CurrentRow.DataBoundItem);
                formDetalle.ShowDialog();
                try
                {
                    Usuario editado = new(formDetalle.Id, formDetalle.Nick, formDetalle.Victorias, formDetalle.Derrotas);                    
                    editar.ModificarUsuario(editado);
                    this.listaUsuarios = editar.ObtenerUsuarios();
                    dgvJugadores.DataSource = listaUsuarios;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }



        //HACER AGREGAR USUARIO
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DataAccess_Usuarios agregar = new();
            DetalleJugador formDetalle = new();
            formDetalle.ShowDialog();

            try
            {
                Usuario nuevoUsuario = new(0, formDetalle.Nick, formDetalle.Victorias, formDetalle.Derrotas);
                agregar.AgregarUsuario(nuevoUsuario);
                this.listaUsuarios = agregar.ObtenerUsuarios();
                dgvJugadores.DataSource = listaUsuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
