using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public class ViewDevicesForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private DataGridView? dgInbentarioa;
        private Button? btMatxura;

        public ViewDevicesForm(Erabiltzailea erabiltzailea)
        {
            this.erabiltzailea = erabiltzailea;
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
            EguneratuTaula();
        }

        private void InterfazeaHasieratu()
        {
            this.Text = "Inbentario Aktiboa (Ekipoak Ondo)";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgInbentarioa = new DataGridView { 
                Dock = DockStyle.Top, 
                Height = 500, 
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            btMatxura = new Button {
                Text = "⚠️ MATXURA JAKINARAZI",
                Location = new Point(50, 530),
                Size = new Size(500, 80),
                BackColor = Color.Orange,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btMatxura.Click += btMatxura_Click;

            Button btIkusiEzabatuak = new Button {
                Text = "🔧 BAJAK ETA MANTENTZEA IKUSI",
                Location = new Point(650, 530),
                Size = new Size(500, 80),
                BackColor = Color.FromArgb(162, 155, 254),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btIkusiEzabatuak.Click += (s, e) => {
                new EzabatuakForm(erabiltzailea).ShowDialog();
                EguneratuTaula(); 
            };

            Button btAldatuKoka = new Button {
                Text = "📍 KOKALEKUA ALDATU",
                Location = new Point(50, 620),
                Size = new Size(1100, 50),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btAldatuKoka.Click += btAldatuKoka_Click;

            this.Controls.Add(dgInbentarioa);
            this.Controls.Add(btMatxura);
            this.Controls.Add(btIkusiEzabatuak);
            this.Controls.Add(btAldatuKoka);
        }

        private void btAldatuKoka_Click(object? sender, EventArgs e)
        {
            if (dgInbentarioa?.SelectedRows.Count > 0)
            {
                var errenkada = dgInbentarioa.SelectedRows[0];
                int id = Convert.ToInt32(errenkada.Cells["Identifikatzailea"].Value);
                string unekoKoka = errenkada.Cells["Kokalekua"].Value?.ToString() ?? "";
                
                // 1. Egiaztatu erabiltzaileak gailu hau mugitzeko baimena duen (iturburu-kokalekuaren arabera)
                bool mugituDezake = false;
                int mintegiId = erabiltzailea.MintegiJabea?.IdMintegia ?? 0;

                if (erabiltzailea.Rola == "IKT arduraduna") mugituDezake = true;
                else if (unekoKoka == "Irakasle Gela") mugituDezake = true;
                else if (mintegiId == 1 && (unekoKoka == "IKT Tailerra" || unekoKoka.StartsWith("PAAG") || unekoKoka.StartsWith("IT") || unekoKoka.StartsWith("MSS"))) mugituDezake = true;
                else if (mintegiId == 2 && unekoKoka.StartsWith("Mekanika")) mugituDezake = true;
                else if (mintegiId == 3 && unekoKoka.StartsWith("Arreta")) mugituDezake = true;
                else if (mintegiId == 4 && unekoKoka.StartsWith("Egurgintza")) mugituDezake = true;

                if (!mugituDezake)
                {
                    MessageBox.Show($"Ezin duzu {unekoKoka} gelako gailurik mugitu. Zure mintegikoak edo Irakasle Gelakoak bakarrik kudeatu ditzakezu.", "Baimenik ez", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // 2. Form txiki bat sortu aukerekin (mugitu nahi den tokia aukeratzeko)
                Form f = new Form { Text = "Aukeratu Kokaleku Berria", Size = new Size(350, 220), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false };
                ComboBox cb = new ComboBox { Location = new Point(30, 30), Size = new Size(270, 30), DropDownStyle = ComboBoxStyle.DropDownList };
                Button btn = new Button { Text = "Gorde", Location = new Point(30, 90), Size = new Size(270, 50), BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };

                var denak = kudeatzailea.LortuKokalekuak();
                bool informatikakoaEdoAdmin = (erabiltzailea.Rola == "IKT arduraduna" || mintegiId == 1);

                foreach (var k in denak)
                {
                    bool baimendua = false;
                    if (k.Value == "IKT Tailerra" && informatikakoaEdoAdmin) baimendua = true;
                    if (k.Value == "Irakasle Gela") baimendua = true;
                    if (mintegiId == 1 && (k.Value.StartsWith("PAAG") || k.Value.StartsWith("IT") || k.Value.StartsWith("MSS"))) baimendua = true;
                    if (mintegiId == 2 && k.Value.StartsWith("Mekanika")) baimendua = true;
                    if (mintegiId == 3 && k.Value.StartsWith("Arreta")) baimendua = true;
                    if (mintegiId == 4 && k.Value.StartsWith("Egurgintza")) baimendua = true;

                    if (baimendua || erabiltzailea.Rola == "IKT arduraduna") cb.Items.Add(k.Value);
                }

                if (cb.Items.Count > 0) cb.SelectedIndex = 0;

                btn.Click += (s, ev) => {
                    if (cb.SelectedItem != null) {
                        kudeatzailea.GailuaKokalekuaAldatu(id, cb.SelectedItem.ToString()!);
                        f.Close();
                        EguneratuTaula();
                    }
                };

                f.Controls.Add(cb); f.Controls.Add(btn);
                f.ShowDialog();
            }
        }

        private void EguneratuTaula()
        {
            if (dgInbentarioa != null)
                dgInbentarioa.DataSource = kudeatzailea.LortuGailuakGuztiak();
        }

        private void btMatxura_Click(object? igorlea, EventArgs e)
        {
            if (dgInbentarioa != null && dgInbentarioa.SelectedRows.Count > 0)
            {
                var errenkada = dgInbentarioa.SelectedRows[0];
                int id = Convert.ToInt32(errenkada.Cells["Identifikatzailea"].Value);
                string marka = errenkada.Cells["Marka"].Value?.ToString() ?? "Gailua";
                string unekoKoka = errenkada.Cells["Kokalekua"].Value?.ToString() ?? "";

                // Egiaztatu erabiltzaileak gailu honen matxura jakinarazteko baimena duen
                bool baimendua = false;
                int mintegiId = erabiltzailea.MintegiJabea?.IdMintegia ?? 0;

                if (erabiltzailea.Rola == "IKT arduraduna") baimendua = true;
                else if (unekoKoka == "Irakasle Gela") baimendua = true;
                else if (mintegiId == 1 && (unekoKoka == "IKT Tailerra" || unekoKoka.StartsWith("PAAG") || unekoKoka.StartsWith("IT") || unekoKoka.StartsWith("MSS"))) baimendua = true;
                else if (mintegiId == 2 && unekoKoka.StartsWith("Mekanika")) baimendua = true;
                else if (mintegiId == 3 && unekoKoka.StartsWith("Arreta")) baimendua = true;
                else if (mintegiId == 4 && unekoKoka.StartsWith("Egurgintza")) baimendua = true;

                if (!baimendua)
                {
                    MessageBox.Show($"Ezin duzu {unekoKoka} gelako gailu baten matxura jakinarazi. Zure mintegikoak edo Irakasle Gelakoak bakarrik kudeatu ditzakezu.", "Baimenik ez", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show($"{marka} gailuaren matxura jakinarazi nahi duzu?\nInbentario aktibotik kenduko da.", "Baieztatu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try {
                        kudeatzailea.GailuaEgoeraAldatu(id, "Hautsia");
                        MessageBox.Show("Matxura jakinarazi da. Ekipoa 'Bajak eta Mantentzea' atalera mugitu da.");
                        EguneratuTaula();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }
    }
}