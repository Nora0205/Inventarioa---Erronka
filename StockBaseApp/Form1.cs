using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public partial class Form1 : Form
    {
        private InbentarioKudeatzailea kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private TextBox txtCPU;
        private NumericUpDown numRAM;
        private ComboBox cmbMintegia;
        private ComboBox cmbKokalekua;
        private Button btnEzabatu;

        public Form1(Erabiltzailea user)
        {
            InitializeComponent();
            kudeatzailea = new InbentarioKudeatzailea();
            erabiltzailea = user;
            AñadirControlesExtra();
            ConfigurarSeguridad();
        }

        private void AñadirControlesExtra()
        {
            // Ocultar el textbox viejo de kokalekua para que no estorbe
            txtKokalekua.Visible = false;

            // CPU
            Label lblCPU = new Label { Text = "CPU:", Location = new Point(417, 216), AutoSize = true };
            txtCPU = new TextBox { Location = new Point(417, 248), Size = new Size(200, 34) };
            
            // RAM
            Label lblRAM = new Label { Text = "RAM (GB):", Location = new Point(230, 63), AutoSize = true };
            numRAM = new NumericUpDown { Location = new Point(230, 100), Size = new Size(100, 34), Minimum = 1, Maximum = 128, Value = 8 };

            // Mintegia
            Label lblMintegia = new Label { Text = "Mintegia:", Location = new Point(230, 138), AutoSize = true };
            cmbMintegia = new ComboBox { Location = new Point(230, 168), Size = new Size(165, 34), DropDownStyle = ComboBoxStyle.DropDownList };

            // Kokalekua (NUEVO COMBOBOX RESTRINGIDO)
            Label lblKoka = new Label { Text = "Kokalekua:", Location = new Point(35, 216), AutoSize = true };
            cmbKokalekua = new ComboBox { Location = new Point(35, 248), Size = new Size(165, 34), DropDownStyle = ComboBoxStyle.DropDownList };
            
            // Ezabatu Button
            btnEzabatu = new Button { 
                Text = "Ezabatu", 
                Location = new Point(1124, 600), 
                Size = new Size(123, 38), 
                BackColor = Color.LightCoral 
            };
            btnEzabatu.Click += btnEzabatu_Click;

            groupBox1.Controls.Add(lblCPU);
            groupBox1.Controls.Add(txtCPU);
            groupBox1.Controls.Add(lblRAM);
            groupBox1.Controls.Add(numRAM);
            groupBox1.Controls.Add(lblMintegia);
            groupBox1.Controls.Add(cmbMintegia);
            groupBox1.Controls.Add(lblKoka);
            groupBox1.Controls.Add(cmbKokalekua);
            this.Controls.Add(btnEzabatu);
        }

        private void ConfigurarSeguridad()
        {
            this.Text = "StockBaseApp - Kaixo, " + erabiltzailea.Izena + " (" + erabiltzailea.Rola + ")";
            txtId.Enabled = false;
            txtId.Text = "AUTO";

            if (erabiltzailea.Rola == "Irakaslea")
            {
                btnGorde.Enabled = false;
                btnEzabatu.Enabled = false;
                groupBox1.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarMintegiak();
            CargarKokalekuak();
            EguneratuTaula();
        }

        private void CargarMintegiak()
        {
            var lista = kudeatzailea.LortuMintegiak();
            cmbMintegia.DisplayMember = "Value";
            cmbMintegia.ValueMember = "Key";
            foreach (var item in lista) cmbMintegia.Items.Add(item);
            if (cmbMintegia.Items.Count > 0) cmbMintegia.SelectedIndex = 0;
        }

        private void CargarKokalekuak()
        {
            var lista = kudeatzailea.LortuKokalekuak();
            cmbKokalekua.DisplayMember = "Value";
            cmbKokalekua.ValueMember = "Key";
            foreach (var item in lista) cmbKokalekua.Items.Add(item);
            if (cmbKokalekua.Items.Count > 0) cmbKokalekua.SelectedIndex = 0;
        }

        private void btnGorde_Click(object sender, EventArgs e)
        {
            try
            {
                string marka = txtMarka.Text;
                DateTime data = dtpErosketa.Value;
                int idMintegia = ((KeyValuePair<int, string>)cmbMintegia.SelectedItem).Key;
                int idKokalekua = ((KeyValuePair<int, string>)cmbKokalekua.SelectedItem).Key;

                if (cmbMota.SelectedItem == null) return;
                string mota = cmbMota.SelectedItem.ToString();

                if (mota == "Inprimagailua")
                {
                    kudeatzailea.GailuaGehitu(new Inprimagailua(0, marka, "", "Aktiboa", data, chkKolorea.Checked) { IdMintegia = idMintegia }, idKokalekua);
                }
                else
                {
                    kudeatzailea.GailuaGehitu(new Ordenagailua(0, marka, "", "Aktiboa", data, txtCPU.Text, (int)numRAM.Value) { IdMintegia = idMintegia }, idKokalekua);
                }

                MessageBox.Show("Gorde da!");
                EguneratuTaula();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }

        private void btnEzabatu_Click(object sender, EventArgs e)
        {
            if (dgvInbentarioa.SelectedRows.Count > 0)
            {
                var row = dgvInbentarioa.SelectedRows[0];
                Gailua gailua = (Gailua)row.DataBoundItem;
                if (MessageBox.Show("Ziur zaude " + gailua.Marka + " ezabatu nahi duzula?", "Baieztatu", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    kudeatzailea.GailuaEzabatu(gailua);
                    EguneratuTaula();
                }
            }
        }

        private void EguneratuTaula()
        {
            dgvInbentarioa.DataSource = null;
            dgvInbentarioa.DataSource = kudeatzailea.LortuGailuak();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }
        private void dgvInbentarioa_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void pictureBox1_Click_1(object sender, EventArgs e) { }
    }
}