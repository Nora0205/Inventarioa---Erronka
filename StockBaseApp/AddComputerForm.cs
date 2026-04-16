using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public class AddComputerForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private TextBox txtMarka, txtModeloa, txtCPU, txtKoka;
        private NumericUpDown numRAM;
        private ComboBox cmbMintegia;
        private DateTimePicker dtpData;

        public AddComputerForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.kudeatzailea = new InbentarioSistema();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Ordenagailu Berria";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblMarka = new Label { Text = "Marka:", Location = new Point(20, 20), AutoSize = true };
            txtMarka = new TextBox { Location = new Point(20, 45), Size = new Size(340, 30) };

            Label lblModeloa = new Label { Text = "Modeloa:", Location = new Point(20, 80), AutoSize = true };
            txtModeloa = new TextBox { Location = new Point(20, 105), Size = new Size(340, 30) };

            Label lblCPU = new Label { Text = "CPU:", Location = new Point(20, 140), AutoSize = true };
            txtCPU = new TextBox { Location = new Point(20, 165), Size = new Size(340, 30) };

            Label lblRAM = new Label { Text = "RAM (GB):", Location = new Point(20, 200), AutoSize = true };
            numRAM = new NumericUpDown { Location = new Point(20, 225), Size = new Size(100, 30), Minimum = 1, Maximum = 128, Value = 8 };

            Label lblMintegia = new Label { Text = "Mintegia:", Location = new Point(140, 200), AutoSize = true };
            cmbMintegia = new ComboBox { Location = new Point(140, 225), Size = new Size(220, 30), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblKoka = new Label { Text = "Kokalekua:", Location = new Point(20, 270), AutoSize = true };
            txtKoka = new TextBox { Location = new Point(20, 295), Size = new Size(340, 30) };

            Label lblData = new Label { Text = "Erosketa Data:", Location = new Point(20, 335), AutoSize = true };
            dtpData = new DateTimePicker { Location = new Point(20, 360), Size = new Size(340, 30) };

            Button btnGorde = new Button { Text = "Ordenagailua Gorde", Location = new Point(20, 410), Size = new Size(340, 40), BackColor = Color.LightBlue };
            btnGorde.Click += btnGorde_Click;

            this.Controls.AddRange(new Control[] { lblMarka, txtMarka, lblModeloa, txtModeloa, lblCPU, txtCPU, lblRAM, numRAM, lblMintegia, cmbMintegia, lblKoka, txtKoka, lblData, dtpData, btnGorde });
            
            CargarMintegiak();
        }

        private void CargarMintegiak()
        {
            var lista = kudeatzailea.LortuMintegiak();
            cmbMintegia.DisplayMember = "Value";
            cmbMintegia.ValueMember = "Key";
            foreach (var item in lista) cmbMintegia.Items.Add(item);
            if (cmbMintegia.Items.Count > 0) cmbMintegia.SelectedIndex = 0;
        }

        private void btnGorde_Click(object sender, EventArgs e)
        {
            try
            {
                int idMintegia = ((KeyValuePair<int, string>)cmbMintegia.SelectedItem).Key;
                var pc = new Ordenagailua(0, txtMarka.Text, txtKoka.Text, "Aktiboa", dtpData.Value, txtCPU.Text, (int)numRAM.Value) 
                { 
                    IdMintegia = idMintegia, 
                    Modeloa = txtModeloa.Text 
                };
                kudeatzailea.GailuaGehitu(pc, txtKoka.Text);
                MessageBox.Show("Ondo gorde da!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }
    }
}