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
        private TextBox? txMarka, txModeloa;
        private CheckBox? chkColor;
        private ComboBox? cbMintegia, cbKoka;
        private DateTimePicker? dtData;

        public AddPrinterForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
        }

        private void InterfazeaHasieratu()
        {
            this.Text = "Inprimagailu Berria";
            this.Size = new Size(400, 480);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbMarka = new Label { Text = "Marka:", Location = new Point(20, 20), AutoSize = true };
            txMarka = new TextBox { Location = new Point(20, 45), Size = new Size(340, 30) };

            Label lbModeloa = new Label { Text = "Modeloa:", Location = new Point(20, 80), AutoSize = true };
            txModeloa = new TextBox { Location = new Point(20, 105), Size = new Size(340, 30) };

            Label lbMintegia = new Label { Text = "Mintegia:", Location = new Point(20, 140), AutoSize = true };
            cbMintegia = new ComboBox { Location = new Point(20, 165), Size = new Size(340, 30), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lbKoka = new Label { Text = "Kokalekua:", Location = new Point(20, 205), AutoSize = true };
            cbKoka = new ComboBox { Location = new Point(20, 230), Size = new Size(340, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cbKoka.SelectedIndexChanged += (s, e) => {
                if (cbKoka.SelectedItem == null || cbMintegia == null) return;
                string koka = ((KeyValuePair<int, string>)cbKoka.SelectedItem).Value;
                
                int? nuevoMintegi = null;
                if (koka.StartsWith("PAAG") || koka.StartsWith("IT") || koka.StartsWith("MSS")) nuevoMintegi = 1;
                else if (koka.StartsWith("Mekanika")) nuevoMintegi = 2;
                else if (koka.StartsWith("Arreta")) nuevoMintegi = 3;
                else if (koka.StartsWith("Egurgintza")) nuevoMintegi = 4;

                if (nuevoMintegi.HasValue) {
                    for (int i = 0; i < cbMintegia.Items.Count; i++) {
                        if (((KeyValuePair<int, string>)cbMintegia.Items[i]).Key == nuevoMintegi.Value) {
                            cbMintegia.SelectedIndex = i;
                            cbMintegia.Enabled = false;
                            break;
                        }
                    }
                } else {
                    cbMintegia.Enabled = true;
                }
            };

            chkColor = new CheckBox { Text = "Koloretakoa da?", Location = new Point(20, 275), AutoSize = true };

            Label lbData = new Label { Text = "Erosketa Data:", Location = new Point(20, 305), AutoSize = true };
            dtData = new DateTimePicker { Location = new Point(20, 330), Size = new Size(340, 30) };

            Button btGorde = new Button { Text = "Inprimagailua Gorde", Location = new Point(20, 385), Size = new Size(340, 40), BackColor = Color.LightGreen };
            btGorde.Click += btGorde_Click;

            this.Controls.Add(lbMarka); this.Controls.Add(txMarka);
            this.Controls.Add(lbModeloa); this.Controls.Add(txModeloa);
            this.Controls.Add(lbMintegia); this.Controls.Add(cbMintegia);
            this.Controls.Add(lbKoka); this.Controls.Add(cbKoka);
            this.Controls.Add(chkColor);
            this.Controls.Add(lbData); this.Controls.Add(dtData);
            this.Controls.Add(btGorde);
            
            DatuakKargatu();
        }

        private void DatuakKargatu()
        {
            var mintegiak = kudeatzailea.LortuMintegiak();
            cbMintegia!.DisplayMember = "Value";
            cbMintegia.ValueMember = "Key";
            foreach (var m in mintegiak) cbMintegia.Items.Add(m);
            if (cbMintegia.Items.Count > 0) cbMintegia.SelectedIndex = 0;

            var kokalekuak = kudeatzailea.LortuKokalekuak();
            bool informatikakoaEdoAdmin = (erabiltzailea.Rola == "IKT arduraduna" || (erabiltzailea.MintegiJabea?.IdMintegia ?? 0) == 1);

            cbKoka!.DisplayMember = "Value";
            cbKoka.ValueMember = "Key";
            foreach (var k in kokalekuak) 
            {
                if (k.Value == "IKT Tailerra" && !informatikakoaEdoAdmin) continue;
                cbKoka.Items.Add(k);
            }
            if (cbKoka.Items.Count > 0) cbKoka.SelectedIndex = 0;
        }

        private void btGorde_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cbMintegia?.SelectedItem == null || cbKoka?.SelectedItem == null || txMarka == null || dtData == null || txModeloa == null || chkColor == null) return;

                int idMintegia = ((KeyValuePair<int, string>)cbMintegia.SelectedItem).Key;
                string kokalekua = ((KeyValuePair<int, string>)cbKoka.SelectedItem).Value;

                var printer = new Inprimagailua(0, txMarka.Text, kokalekua, "Aktiboa", dtData.Value, chkColor.Checked) 
                { 
                    IdMintegia = idMintegia, 
                    Modeloa = txModeloa.Text 
                };
                kudeatzailea.GailuaGehitu(printer, kokalekua);
                MessageBox.Show("Inprimagailua ondo gorde da!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }
    }
}