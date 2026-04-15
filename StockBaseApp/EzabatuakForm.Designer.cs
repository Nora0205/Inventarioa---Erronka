namespace StockBaseApp
{
    partial class EzabatuakForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvEzabatuak;
        private System.Windows.Forms.Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvEzabatuak = new System.Windows.Forms.DataGridView();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEzabatuak)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 21);
            this.lblTitulo.Text = "Ezabatutako Gailuen Historikoa";

            // dgvEzabatuak
            this.dgvEzabatuak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEzabatuak.Location = new System.Drawing.Point(20, 60);
            this.dgvEzabatuak.Name = "dgvEzabatuak";
            this.dgvEzabatuak.ReadOnly = true;
            this.dgvEzabatuak.Size = new System.Drawing.Size(740, 350);
            this.dgvEzabatuak.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);

            // EzabatuakForm
            this.ClientSize = new System.Drawing.Size(780, 450);
            this.Controls.Add(this.dgvEzabatuak);
            this.Controls.Add(this.lblTitulo);
            this.Name = "EzabatuakForm";
            this.Text = "StockBase - Historikoa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgvEzabatuak)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}