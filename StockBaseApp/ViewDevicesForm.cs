using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public class ViewDevicesForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private DataGridView dgvInbentarioa;

        public ViewDevicesForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.kudeatzailea = new InbentarioSistema();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gailuen Inbentarioa";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvInbentarioa = new DataGridView { 
                Location = new Point(20, 20), 
                Size = new Size(940, 450), 
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Button btnEzabatu = new Button { Text = "Hautatutakoa Ezabatu", Location = new Point(20, 490), Size = new Size(200, 40), BackColor = Color.LightCoral };
            Button btnHistorial = new Button { Text = "Baxen Historiala Ikusi", Location = new Point(240, 490), Size = new Size(200, 40), BackColor = Color.LightGray };
            Button btnCerrar = new Button { Text = "Itzuli", Location = new Point(760, 490), Size = new Size(200, 40) };

            btnEzabatu.Click += btnEzabatu_Click;
            btnHistorial.Click += (s, e) => new EzabatuakForm().ShowDialog();
            btnCerrar.Click += (s, e) => this.Close();

            // Segurtasuna
            if (erabiltzailea.Rola == "Irakaslea") btnEzabatu.Enabled = false;

            this.Controls.Add(dgvInbentarioa);
            this.Controls.Add(btnEzabatu);
            this.Controls.Add(btnHistorial);
            this.Controls.Add(btnCerrar);

            EguneratuTaula();
        }

        private void EguneratuTaula()
        {
            int? filtro = (erabiltzailea.Rola != "Admin" && erabiltzailea.Rola != "IKT arduraduna") ? erabiltzailea.MintegiJabea?.IdMintegia : null;
            dgvInbentarioa.DataSource = kudeatzailea.LortuGailuakGuztiak(filtro);
        }

        private void btnEzabatu_Click(object sender, EventArgs e)
        {
            if (dgvInbentarioa.SelectedRows.Count > 0)
            {
                var row = dgvInbentarioa.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["ID"].Value);
                string marka = row.Cells["Marka"].Value.ToString() ?? "";
                string mota = row.Cells["Mota"].Value.ToString() ?? "";
                string koka = row.Cells["Kokalekua"].Value.ToString() ?? "";
                DateTime data = Convert.ToDateTime(row.Cells["Data"].Value);

                if (MessageBox.Show($"{marka} ezabatu nahi duzu?", "Baieztatu", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Gailua temp = (mota == "Ordenagailua") ? new Ordenagailua(id, marka, koka, "Ezabatua", data, "", 0) : (Gailua)new Inprimagailua(id, marka, koka, "Ezabatua", data, false);
                    kudeatzailea.GailuaEzabatu(temp);
                    EguneratuTaula();
                }
            }
        }
    }
}