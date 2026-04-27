using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    /// <summary>
    /// Mintegiak (departamentuak) kudeatzeko formularioa.
    /// Administratzaileei mintegi berriak sortzeko eta daudenak ezabatzeko aukera ematen die.
    /// </summary>
    public class ManageMintegiakForm : Form
    {
        private readonly Erabiltzailea admin;
        private readonly InbentarioSistema kudeatzailea;
        private ListBox? lstMintegiak;
        private TextBox? txtIzena;
        private Color primaryColor = Color.FromArgb(108, 92, 231);

        /// <summary>
        /// ManageMintegiakForm-en instantzia berri bat sortzen du.
        /// </summary>
        /// <param name="admin">Ekintzak burutuko dituen administratzailearen objektua.</param>
        public ManageMintegiakForm(Erabiltzailea admin)
        {
            this.admin = admin;
            this.kudeatzailea = new InbentarioSistema();
            InitializeComponent();
            CargarMintegiak();
        }

        /// <summary>
        /// Formularioaren interfaze grafikoa inizializatzen du.
        /// </summary>
        private void InitializeComponent()
        {
            // (Interfazearen konfigurazio kodea...)
            this.Text = "Mintegiak Kudeatu";
            this.Size = new Size(400, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Panel headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = primaryColor };
            Label lblTitle = new Label { 
                Text = "MINTEGIAK", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter 
            };
            headerPanel.Controls.Add(lblTitle);

            Label lblBerria = new Label { Text = "Mintegi Berria:", Location = new Point(20, 75), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtIzena = new TextBox { Location = new Point(20, 100), Size = new Size(240, 30), Font = new Font("Segoe UI", 11) };
            
            Button btnGehitu = new Button {
                Text = "➕ Gehitu", Location = new Point(270, 98), Size = new Size(95, 33),
                BackColor = Color.FromArgb(0, 184, 148), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnGehitu.FlatAppearance.BorderSize = 0;
            btnGehitu.Click += BtnGehitu_Click;

            Label lblZerrenda = new Label { Text = "Dauden Mintegiak:", Location = new Point(20, 150), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lstMintegiak = new ListBox {
                Location = new Point(20, 175), Size = new Size(345, 280),
                Font = new Font("Segoe UI", 11)
            };

            Button btnEzabatu = new Button {
                Text = "🗑️ Ezabatu Hautatutakoa", Location = new Point(20, 475), Size = new Size(345, 45),
                BackColor = Color.FromArgb(214, 48, 49), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnEzabatu.FlatAppearance.BorderSize = 0;
            btnEzabatu.Click += BtnEzabatu_Click;

            this.Controls.AddRange(new Control[] { headerPanel, lblBerria, txtIzena, btnGehitu, lblZerrenda, lstMintegiak, btnEzabatu });
        }

        /// <summary>
        /// Datu-basetik mintegi guztiak kargatzen ditu eta zerrendan erakusten ditu.
        /// Sistema-mintegiak (1 eta 99) ez dira erakusten.
        /// </summary>
        private void CargarMintegiak()
        {
            if (lstMintegiak == null) return;
            lstMintegiak.Items.Clear();
            var list = kudeatzailea.LortuMintegiak();
            lstMintegiak.DisplayMember = "Value";
            lstMintegiak.ValueMember = "Key";
            foreach (var item in list)
            {
                if (item.Key != 1 && item.Key != 99) 
                    lstMintegiak.Items.Add(item);
            }
        }

        /// <summary>
        /// Mintegi berria gehitzeko botoiaren klik ekintza kudeatzen du.
        /// </summary>
        private void BtnGehitu_Click(object? sender, EventArgs e)
        {
            if (txtIzena == null || string.IsNullOrWhiteSpace(txtIzena.Text))
            {
                MessageBox.Show("Idatzi mintegiaren izena.", "Kontuz", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                kudeatzailea.MintegiaGehitu(txtIzena.Text, admin.IdErabiltzailea);
                MessageBox.Show("Mintegia ondo sortu da.", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtIzena.Text = "";
                CargarMintegiak();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hautatutako mintegia ezabatzeko botoiaren klik ekintza kudeatzen du.
        /// </summary>
        private void BtnEzabatu_Click(object? sender, EventArgs e)
        {
            if (lstMintegiak?.SelectedItem is KeyValuePair<int, string> selected)
            {
                var result = MessageBox.Show($"Ziur zaude '{selected.Value}' mintegia ezabatu nahi duzula?\nGailuak 'Birbanatu' mintegira mugituko dira.", 
                                           "Mintegia Ezabatu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        kudeatzailea.MintegiaEzabatu(selected.Key, admin.IdErabiltzailea);
                        MessageBox.Show("Mintegia ondo ezabatu da.", "Arrakasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMintegiak();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hautatu mintegi bat zerrendatik.", "Kontuz", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}