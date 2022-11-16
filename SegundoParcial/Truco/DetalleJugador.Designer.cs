
namespace Truco
{
    partial class DetalleJugador
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.txtVictorias = new System.Windows.Forms.TextBox();
            this.txtDerrotas = new System.Windows.Forms.TextBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "NickName :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Victorias :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Derrotas :";
            // 
            // txtNick
            // 
            this.txtNick.Enabled = false;
            this.txtNick.Location = new System.Drawing.Point(120, 4);
            this.txtNick.MaxLength = 25;
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(126, 23);
            this.txtNick.TabIndex = 4;
            // 
            // txtVictorias
            // 
            this.txtVictorias.Location = new System.Drawing.Point(120, 42);
            this.txtVictorias.MaxLength = 3;
            this.txtVictorias.Name = "txtVictorias";
            this.txtVictorias.Size = new System.Drawing.Size(126, 23);
            this.txtVictorias.TabIndex = 5;
            this.txtVictorias.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVictorias_KeyPress);
            // 
            // txtDerrotas
            // 
            this.txtDerrotas.Location = new System.Drawing.Point(120, 80);
            this.txtDerrotas.MaxLength = 3;
            this.txtDerrotas.Name = "txtDerrotas";
            this.txtDerrotas.Size = new System.Drawing.Size(126, 23);
            this.txtDerrotas.TabIndex = 6;
            this.txtDerrotas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDerrotas_KeyPress);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(35, 109);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(211, 46);
            this.btnConfirmar.TabIndex = 8;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // DetalleJugador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(278, 163);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.txtDerrotas);
            this.Controls.Add(this.txtVictorias);
            this.Controls.Add(this.txtNick);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(294, 202);
            this.MinimumSize = new System.Drawing.Size(294, 202);
            this.Name = "DetalleJugador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datos del jugador";
            this.Load += new System.EventHandler(this.DetalleJugador_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVictorias;
        private System.Windows.Forms.TextBox txtDerrotas;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        public System.Windows.Forms.TextBox txtNick;
    }
}