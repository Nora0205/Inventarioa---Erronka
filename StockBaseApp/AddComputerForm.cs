using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    /// <summary>
    /// Ordenagailu berriak sisteman gehitzeko erabiltzen den inprimakia.
    /// </summary>
    public class AddComputerForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private TextBox? txMarka, txModeloa, txCPU;
        private ComboBox? cbKoka, cbMintegia;
        private NumericUpDown? nmRAM;
        private DateTimePicker? dtData;

        /// <summary>
        /// AddComputerForm klasearen instantzia berria sortzen du.
        /// </summary>
        /// <param name="user">Inprimakia erabiltzen ari den erabiltzailea.</param>
        public AddComputerForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
        }

        /// <summary>
        /// Inprimakiaren interfaze grafikoa eta kontrolak hasten ditu.
        /// </summary>
        private void InterfazeaHasieratu()
        {
            this.Text = "Ordenagailu Berria";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbMarka = new Label { Text = "Marka:", Location = new Point(20, 20), AutoSize = true };
            txMarka = new TextBox { Location = new Point(20, 45), Size = new Size(340, 30) };

            Label lbModeloa = new Label { Text = "Modeloa:", Location = new Point(20, 80), AutoSize = true };
            txModeloa = new TextBox { Location = new Point(20, 105), Size = new Size(340, 30) };

            Label lbCPU = new Label { Text = "CPU:", Location = new Point(20, 140), AutoSize = true };
            txCPU = new TextBox { Location = new Point(20, 165), Size = new Size(340, 30) };

            Label lbRAM = new Label { Text = "RAM (GB):", Location = new Point(20, 200), AutoSize = true };
            nmRAM = new NumericUpDown { Location = new Point(20, 225), Size = new Size(100, 30), Minimum = 1, Maximum = 128, Value = 8 };

            Label lbMintegia = new Label { Text = "Mintegia:", Location = new Point(140, 200), AutoSize = true };
            cbMintegia = new ComboBox { Location = new Point(140, 225), Size = new Size(220, 30), DropDownStyle = ComboBoxStyle.DropDownList };

            Label lbKoka = new Label { Text = "Kokalekua:", Location = new Point(20, 270), AutoSize = true };
            cbKoka = new ComboBox { Location = new Point(20, 295), Size = new Size(340, 30), DropDownStyle = ComboBoxStyle.DropDownList };
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
                        if (cbMintegia.Items[i] is KeyValuePair<int, string> item && item.Key == nuevoMintegi.Value) {
                            cbMintegia.SelectedIndex = i;
                            cbMintegia.Enabled = false; 
                            break;
                        }
                    }
                } else {
                  
                    cbMintegia.Enabled = true;
                }
            };

            Label lbData = new Label { Text = "Erosketa Data:", Location = new Point(20, 335), AutoSize = true };
            dtData = new DateTimePicker { Location = new Point(20, 360), Size = new Size(340, 30) };

            Button btGorde = new Button { Text = "Ordenagailua Gorde", Location = new Point(20, 410), Size = new Size(340, 40), BackColor = Color.LightBlue };
            btGorde.Click += btGorde_Click;

            this.Controls.Add(lbMarka); this.Controls.Add(txMarka);
            this.Controls.Add(lbModeloa); this.Controls.Add(txModeloa);
            this.Controls.Add(lbCPU); this.Controls.Add(txCPU);
            this.Controls.Add(lbRAM); this.Controls.Add(nmRAM);
            this.Controls.Add(lbMintegia); this.Controls.Add(cbMintegia);
            this.Controls.Add(lbKoka); this.Controls.Add(cbKoka);
            this.Controls.Add(lbData); this.Controls.Add(dtData);
            this.Controls.Add(btGorde);
            
            DatuakKargatu();
        }

        /// <summary>
        /// Mintegi eta kokaleku datuak datu-basetik kargatzen ditu eta ComboBox-etan bistaratzen ditu.
        /// </summary>
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

        /// <summary>
        /// Gorde botoia sakatzean exekutatzen da. Sartutako datuak balidatzen ditu eta ordenagailu berria datu-basean gordetzen du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen objektua.</param>
        /// <param name="e">Gertaeraren datuak.</param>
        private void btGorde_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cbMintegia?.SelectedItem == null || cbKoka?.SelectedItem == null || txMarka == null || dtData == null || txModeloa == null || txCPU == null || nmRAM == null) return;

                if (string.IsNullOrWhiteSpace(txMarka.Text) || string.IsNullOrWhiteSpace(txModeloa.Text) || string.IsNullOrWhiteSpace(txCPU.Text))
                {
                    MessageBox.Show("Mesedez, bete eremu guztiak.", "Datuak falta dira", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idMintegia = ((KeyValuePair<int, string>)cbMintegia.SelectedItem).Key;
                string kokalekua = ((KeyValuePair<int, string>)cbKoka.SelectedItem).Value;

                var pc = new Ordenagailua(0, txMarka.Text, kokalekua, "Aktiboa", dtData.Value, txCPU.Text, (int)nmRAM.Value) 
                { 
                    IdMintegia = idMintegia, 
                    Modeloa = txModeloa.Text 
                };
                kudeatzailea.GailuaGehitu(pc, kokalekua, erabiltzailea.IdErabiltzailea);
                MessageBox.Show("Ondo gorde da!");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }
    }
}