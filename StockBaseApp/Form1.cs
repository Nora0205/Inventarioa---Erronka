using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public partial class Form1 : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;

        // Controles dinámicos
        private TextBox txtCPU = null!;
        private TextBox txtModeloa = null!;
        private NumericUpDown numRAM = null!;
        private ComboBox cmbMintegia = null!;
        private ComboBox cmbKokalekua = null!;
        private Button btnEzabatu = null!;
        private Button btnEsportatu = null!;
        private Button btnVerEzabatuak = null!;
        private Label lblCPU = null!;
        private Label lblRAM = null!;
        private Label lblMintegia = null!;
        private Label lblModeloa = null!;
        private Label lblKoka = null!;

        public Form1(Erabiltzailea user)
        {
            InitializeComponent();
            kudeatzailea = new InbentarioSistema();
            erabiltzailea = user;

            PrepararInterfazDinamica();
            ConfigurarSeguridad();

            cmbMota.SelectedIndexChanged += cmbMota_SelectedIndexChanged;
            btnGorde.Click += btnGorde_Click;
        }

        private void PrepararInterfazDinamica()
        {
            txtKokalekua.Visible = false;
            label3.Visible = false;

            lblModeloa = new Label { Text = "Modeloa:", Location = new Point(35, 175), AutoSize = true };
            txtModeloa = new TextBox { Location = new Point(35, 205), Size = new Size(165, 34) };

            lblKoka = new Label { Text = "Kokalekua:", Location = new Point(35, 240), AutoSize = true };
            cmbKokalekua = new ComboBox { Location = new Point(35, 270), Size = new Size(165, 34), DropDownStyle = ComboBoxStyle.DropDownList };

            lblRAM = new Label { Text = "RAM (GB):", Location = new Point(230, 57), AutoSize = true, Visible = false };
            numRAM = new NumericUpDown { Location = new Point(230, 85), Size = new Size(100, 34), Minimum = 1, Maximum = 128, Value = 8, Visible = false };

            lblMintegia = new Label { Text = "Mintegia:", Location = new Point(230, 138), AutoSize = true };
            cmbMintegia = new ComboBox { Location = new Point(230, 168), Size = new Size(165, 34), DropDownStyle = ComboBoxStyle.DropDownList };

            lblCPU = new Label { Text = "CPU (Prozesagailua):", Location = new Point(417, 138), AutoSize = true, Visible = false };
            txtCPU = new TextBox { Location = new Point(417, 168), Size = new Size(200, 34), Visible = false };

            btnEzabatu = new Button { Text = "Ezabatu", Location = new Point(1124, 600), Size = new Size(123, 38), BackColor = Color.LightCoral };
            btnEzabatu.Click += btnEzabatu_Click;

            btnEsportatu = new Button { Text = "Esportatu (CSV)", Location = new Point(980, 600), Size = new Size(130, 38), BackColor = Color.LightBlue };
            btnEsportatu.Click += btnEsportatu_Click;

            // Nuevo botón Historial
            btnVerEzabatuak = new Button { 
                Text = "Ikusi Ezabatuak", 
                Location = new Point(840, 600), 
                Size = new Size(130, 38), 
                BackColor = Color.LightGray 
            };
            btnVerEzabatuak.Click += btnVerEzabatuak_Click;

            chkKolorea.Visible = false;

            groupBox1.Controls.Add(lblModeloa);
            groupBox1.Controls.Add(txtModeloa);
            groupBox1.Controls.Add(lblKoka);
            groupBox1.Controls.Add(cmbKokalekua);
            groupBox1.Controls.Add(lblCPU);
            groupBox1.Controls.Add(txtCPU);
            groupBox1.Controls.Add(lblRAM);
            groupBox1.Controls.Add(numRAM);
            groupBox1.Controls.Add(lblMintegia);
            groupBox1.Controls.Add(cmbMintegia);

            this.Controls.Add(btnEzabatu);
            this.Controls.Add(btnEsportatu);
            this.Controls.Add(btnVerEzabatuak);
        }

        private void btnVerEzabatuak_Click(object? sender, EventArgs e)
        {
            EzabatuakForm form = new EzabatuakForm();
            form.ShowDialog();
        }

        private void ConfigurarSeguridad()
        {
            this.Text = "StockBaseApp - Kaixo, " + erabiltzailea.Izena + " (" + erabiltzailea.Rola + ")";
            txtId.Enabled = false;
            txtId.Text = "AUTO";

            if (erabiltzailea.Rola != "Admin" && erabiltzailea.Rola != "IKT arduraduna")
            {
                cmbMintegia.Enabled = false;
            }

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

            if (erabiltzailea.MintegiJabea != null && !cmbMintegia.Enabled)
            {
                for (int i = 0; i < cmbMintegia.Items.Count; i++)
                {
                    if (((KeyValuePair<int, string>)cmbMintegia.Items[i]).Key == erabiltzailea.MintegiJabea.IdMintegia)
                    {
                        cmbMintegia.SelectedIndex = i;
                        break;
                    }
                }
            }

            EguneratuTaula();
        }

        private void CargarMintegiak()
        {
            var lista = kudeatzailea.LortuMintegiak();
            cmbMintegia.DisplayMember = "Value";
            cmbMintegia.ValueMember = "Key";
            foreach (var item in lista) cmbMintegia.Items.Add(item);
            if (cmbMintegia.Items.Count > 0 && cmbMintegia.SelectedIndex == -1) cmbMintegia.SelectedIndex = 0;
        }

        private void CargarKokalekuak()
        {
            var lista = kudeatzailea.LortuKokalekuak();
            cmbKokalekua.DisplayMember = "Value";
            cmbKokalekua.ValueMember = "Key";
            foreach (var item in lista) cmbKokalekua.Items.Add(item);
            if (cmbKokalekua.Items.Count > 0) cmbKokalekua.SelectedIndex = 0;
        }

        private void EguneratuTaula()
        {
            dgvInbentarioa.DataSource = null;
            int? filtroMintegia = null;
            if (erabiltzailea.Rola != "Admin" && erabiltzailea.Rola != "IKT arduraduna")
            {
                filtroMintegia = erabiltzailea.MintegiJabea?.IdMintegia;
            }
            dgvInbentarioa.DataSource = kudeatzailea.LortuGailuakGuztiak(filtroMintegia);
        }

        private void btnGorde_Click(object? sender, EventArgs e)
        {
            try
            {
                string marka = txtMarka.Text;
                string modeloa = txtModeloa.Text;
                DateTime data = dtpErosketa.Value;

                if (cmbMintegia.SelectedItem == null || cmbKokalekua.SelectedItem == null) { MessageBox.Show("Hautatu mintegia eta kokalekua."); return; }

                int idMintegia = ((KeyValuePair<int, string>)cmbMintegia.SelectedItem).Key;
                int idKokalekua = ((KeyValuePair<int, string>)cmbKokalekua.SelectedItem).Key;

                if (cmbMota.SelectedItem == null) return;
                string mota = cmbMota.SelectedItem.ToString() ?? "";

                if (mota == "Inprimagailua")
                    kudeatzailea.GailuaGehitu(new Inprimagailua(0, marka, "", "Aktiboa", data, chkKolorea.Checked) { IdMintegia = idMintegia, Modeloa = modeloa }, idKokalekua);
                else
                    kudeatzailea.GailuaGehitu(new Ordenagailua(0, marka, "", "Aktiboa", data, txtCPU.Text, (int)numRAM.Value) { IdMintegia = idMintegia, Modeloa = modeloa }, idKokalekua);

                MessageBox.Show("Gorde da!");
                EguneratuTaula();
                LimpiarFormulario();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }

        private void btnEzabatu_Click(object? sender, EventArgs e)
        {
            if (dgvInbentarioa.SelectedRows.Count > 0)
            {
                var row = dgvInbentarioa.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["ID"].Value);
                string marka = row.Cells["Marka"].Value.ToString() ?? "";
                string mota = row.Cells["Mota"].Value.ToString() ?? "";

                if (MessageBox.Show("Ziur zaude " + marka + " ezabatu nahi duzula?", "Baieztatu", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Gailua temp = (mota == "Ordenagailua") ? new Ordenagailua(id, marka, "", "", DateTime.Now, "", 0) : (Gailua)new Inprimagailua(id, marka, "", "", DateTime.Now, false);
                    kudeatzailea.GailuaEzabatu(temp);
                    EguneratuTaula();
                }
            }
        }

        private void btnEsportatu_Click(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV fitxategia|*.csv", FileName = "Inbentarioa_" + DateTime.Now.ToString("yyyyMMdd") + ".csv" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    int? filtro = (erabiltzailea.Rola != "Admin" && erabiltzailea.Rola != "IKT arduraduna") ? erabiltzailea.MintegiJabea?.IdMintegia : null;
                    DataTable dt = kudeatzailea.LortuGailuakGuztiak(filtro);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    string[] headers = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                    sb.AppendLine(string.Join(";", headers));
                    foreach (DataRow row in dt.Rows) sb.AppendLine(string.Join(";", row.ItemArray.Select(f => f?.ToString()?.Replace(";", ",") ?? "")));
                    System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                    MessageBox.Show("Datuak ondo esportatu dira.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Errorea esportatzerakoan: " + ex.Message); }
        }

        private void cmbMota_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbMota.SelectedItem == null) return;
            string mota = cmbMota.SelectedItem.ToString() ?? "";
            bool isPC = (mota == "Ordenagailua");
            lblCPU.Visible = txtCPU.Visible = lblRAM.Visible = numRAM.Visible = isPC;
            chkKolorea.Visible = !isPC;
        }

        private void LimpiarFormulario()
        {
            txtMarka.Clear(); txtModeloa.Clear(); txtCPU.Clear(); numRAM.Value = 8; chkKolorea.Checked = false; cmbMota.SelectedIndex = -1;
            lblCPU.Visible = txtCPU.Visible = lblRAM.Visible = numRAM.Visible = chkKolorea.Visible = false;
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
        private void chkKolorea_CheckedChanged(object sender, EventArgs e) { }
        private void cmbMota_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void txtMarka_TextChanged(object sender, EventArgs e) { }
    }
}