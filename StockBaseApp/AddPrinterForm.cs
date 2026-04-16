using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public class AddPrinterForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private TextBox txtMarka, txtModeloa, txtKoka;
        private CheckBox chkColor;
        private ComboBox cmbMintegia;
        private DateTimePicker dtpData;

        public AddPrinterForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.kudeatzailea = new InbentarioSistema();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Inprimagailu Berria";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblMarka = new Label { Text = "Marka:", Location = new Point(20, 20), AutoSize = true };
            txtMarka = new TextBox { Location = new Point(20, 45), Size = new Size(340, 30) };

            Label lblModeloa = new Label { Text = "Modeloa:", Location = new Point(20, 80), AutoSize = true };
            txtModeloa = new TextBox { Location = new Point(20, 105), Size = new Size(340, 30) };

            Label lblKoka = new Label { Text = "Kokalekua:", Location = new Point(20, 140), AutoSize = true };
            txtKoka = new TextBox { Location = new Point(20, 165), Size = new Size(340, 30) };

            Label lblMintegia = new Label { Text = "Mintegia:", Location = new Point(20, 205), AutoSize = true };
            cmbMintegia = new ComboBox { Location = new Point(20, 230), Size = new Size(340, 30), DropDownStyle = ComboBoxStyle.DropDownList };

            chkColor = new CheckBox { Text = "Koloretakoa da?", Location = new Point(20, 275), AutoSize = true };

            Label lblData = new Label { Text = "Erosketa Data:", Location = new Point(20, 305), AutoSize = true };
            dtpData = new DateTimePicker { Location = new Point(20, 330), Size = new Size(340, 30) };

            Button btnGorde = new Button { Text = "Inprimagailua Gorde", Location = new Point(20, 370), Size = new Size(340, 40), BackColor = Color.LightGreen };
            btnGorde.Click += btnGorde_Click;

            this.Controls.AddRange(new Control[] { lblMarka, txtMarka, lblModeloa, txtModeloa, lblKoka, txtKoka, lblMintegia, cmbMintegia, chkColor, lblData, dtpData, btnGorde });
            
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
                var printer = new Inprimagailua(0, txtMarka.Text, txtKoka.Text, "Aktiboa", dtpData.Value, chkColor.Checked) 
                { 
                    IdMintegia = idMintegia, 
                    Modeloa = txtModeloa.Text 
                };
                kudeatzailea.GailuaGehitu(printer, txtKoka.Text);
                MessageBox.Show("Ondo gorde da!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }
    }
}