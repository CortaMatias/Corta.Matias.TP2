
namespace Truco
{
    partial class CrearSala
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbJugador1 = new System.Windows.Forms.ComboBox();
            this.cmbJugador2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SteelBlue;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(71, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Crear Sala";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "NickName jugador 1 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "NickName jugador 2 :";
            // 
            // cmbJugador1
            // 
            this.cmbJugador1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbJugador1.FormattingEnabled = true;
            this.cmbJugador1.Location = new System.Drawing.Point(212, 51);
            this.cmbJugador1.Name = "cmbJugador1";
            this.cmbJugador1.Size = new System.Drawing.Size(140, 25);
            this.cmbJugador1.TabIndex = 5;
            this.cmbJugador1.SelectedIndexChanged += new System.EventHandler(this.cmbJugador1_SelectedIndexChanged);
            // 
            // cmbJugador2
            // 
            this.cmbJugador2.Enabled = false;
            this.cmbJugador2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbJugador2.FormattingEnabled = true;
            this.cmbJugador2.Location = new System.Drawing.Point(212, 124);
            this.cmbJugador2.Name = "cmbJugador2";
            this.cmbJugador2.Size = new System.Drawing.Size(140, 25);
            this.cmbJugador2.TabIndex = 6;
            this.cmbJugador2.SelectedIndexChanged += new System.EventHandler(this.cmbJugador2_SelectedIndexChanged);
            // 
            // CrearSala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Truco.Properties.Resources.Aver__;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(388, 234);
            this.Controls.Add(this.cmbJugador2);
            this.Controls.Add(this.cmbJugador1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.MaximumSize = new System.Drawing.Size(404, 273);
            this.MinimumSize = new System.Drawing.Size(404, 273);
            this.Name = "CrearSala";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crear Sala";
            this.Load += new System.EventHandler(this.CrearSala_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbJugador1;
        private System.Windows.Forms.ComboBox cmbJugador2;
    }
}