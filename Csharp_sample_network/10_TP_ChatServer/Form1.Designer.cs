namespace _10_TP_ChatServer
{
    partial class FrmServer
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDemarrer = new System.Windows.Forms.Button();
            this.btnArreter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(10, 5);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(327, 265);
            this.txtLog.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnArreter);
            this.groupBox1.Controls.Add(this.btnDemarrer);
            this.groupBox1.Location = new System.Drawing.Point(16, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 56);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Commandes";
            // 
            // btnDemarrer
            // 
            this.btnDemarrer.Location = new System.Drawing.Point(237, 16);
            this.btnDemarrer.Name = "btnDemarrer";
            this.btnDemarrer.Size = new System.Drawing.Size(82, 30);
            this.btnDemarrer.TabIndex = 0;
            this.btnDemarrer.Text = "Démarrer";
            this.btnDemarrer.UseVisualStyleBackColor = true;
            this.btnDemarrer.Click += new System.EventHandler(this.btnDemarrer_Click);
            // 
            // btnArreter
            // 
            this.btnArreter.Location = new System.Drawing.Point(154, 21);
            this.btnArreter.Name = "btnArreter";
            this.btnArreter.Size = new System.Drawing.Size(71, 24);
            this.btnArreter.TabIndex = 1;
            this.btnArreter.Text = "Arrêter";
            this.btnArreter.UseVisualStyleBackColor = true;
            this.btnArreter.Click += new System.EventHandler(this.btnArreter_Click);
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 336);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLog);
            this.Name = "FrmServer";
            this.Text = "TP - Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServer_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnArreter;
        private System.Windows.Forms.Button btnDemarrer;
    }
}

