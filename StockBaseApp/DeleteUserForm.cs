using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    /// <summary>
    /// Erabiltzaileak sistematik ezabatzeko inprimakia.
    /// </summary>
    public class DeleteUserForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea admin;
        private DataGridView? dgErabiltzaileak;

        /// <summary>
        /// DeleteUserForm klasearen instantzia berria sortzen du.
        /// </summary>
        /// <param name="admin">Ekintza burutzen ari den administratzailea.</param>
        public DeleteUserForm(Erabiltzailea admin)
        {
            this.admin = admin;
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
            EguneratuTaula();
        }

        /// <summary>
        /// Inprimakiaren interfaze grafikoa eta kontrolak hasieratzen ditu.
        /// </summary>
        private void InterfazeaHasieratu()
        {
            this.Text = "Erabiltzailea Ezabatu";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgErabiltzaileak = new DataGridView { 
                Dock = DockStyle.Top, 
                Height = 350, 
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Button btEzabatu = new Button { 
                Text = "🗑️ HAUTATUTAKOA EZABATU", 
                Location = new Point(150, 380),
                Size = new Size(300, 50),
                BackColor = Color.Salmon,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btEzabatu.Click += btEzabatu_Click;

            this.Controls.Add(dgErabiltzaileak);
            this.Controls.Add(btEzabatu);
        }

        /// <summary>
        /// Erabiltzaileen taula datu-baseko informazioarekin eguneratzen du.
        /// </summary>
        private void EguneratuTaula()
        {
            if (dgErabiltzaileak != null)
                dgErabiltzaileak.DataSource = kudeatzailea.ErabiltzaileGuztiakLortu();
        }

        /// <summary>
        /// Ezabatu botoia sakatzean exekutatzen da. Hautatutako erabiltzailea sistematik ezabatzen du.
        /// </summary>
        /// <param name="igorlea">Gertaera sortu duen objektua.</param>
        /// <param name="e">Gertaeraren datuak.</param>
        private void btEzabatu_Click(object? igorlea, EventArgs e)
        {
            if (dgErabiltzaileak != null && dgErabiltzaileak.SelectedRows.Count > 0)
            {
                var errenkada = dgErabiltzaileak.SelectedRows[0];
                int id = Convert.ToInt32(errenkada.Cells["Identifikatzailea"].Value);
                string izena = errenkada.Cells["Izena"].Value?.ToString() ?? "";

                if (MessageBox.Show($"{izena} ezabatu nahi duzu?", "Baieztatu", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try {
                        kudeatzailea.ErabiltzaileaEzabatu(id, izena, admin.IdErabiltzailea);
                        MessageBox.Show("Erabiltzailea ezabatua.");
                        EguneratuTaula();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }
    }
}