using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    /// <summary>
    /// Baja emandako edo mantenimenduan dauden gailuak kudeatzeko inprimakia.
    /// </summary>
    public partial class EzabatuakForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea erabiltzailea;
        private DataGridView? dgBajaMantentze;
        private Button? btBerreskuratu, btBehinBetiko, btMantenimendura;

        /// <summary>
        /// EzabatuakForm klasearen instantzia berria sortzen du.
        /// </summary>
        /// <param name="erabiltzailea">Inprimakia erabiltzen ari den erabiltzailea.</param>
        public EzabatuakForm(Erabiltzailea erabiltzailea)
        {
            this.erabiltzailea = erabiltzailea;
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaEskuzHasieratu();
            EguneratuTaula();
        }

        /// <summary>
        /// Inprimakiaren interfaze grafikoa eta kontrolak eskuz hasieratzen ditu.
        /// </summary>
        private void InterfazeaEskuzHasieratu()
        {
            this.Text = "Bajak eta Mantenimendua";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgBajaMantentze = new DataGridView { 
                Dock = DockStyle.Top, 
                Height = 450, 
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White
            };
            dgBajaMantentze.SelectionChanged += (s, e) => EguneratuBotoiak();

            btBerreskuratu = new Button {
                Text = "✅ AKTIBATU (KONPONDUA)",
                Location = new Point(50, 500),
                Size = new Size(350, 70),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Visible = (erabiltzailea.Rola != "Irakaslea")
            };
            btBerreskuratu.Click += btBerreskuratu_Click;

            btMantenimendura = new Button {
                Text = "🔧 MANTENTZE-LANETARA",
                Location = new Point(425, 500),
                Size = new Size(350, 70),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Visible = (erabiltzailea.Rola != "Irakaslea")
            };
            btMantenimendura.Click += btMantenimendura_Click;

            btBehinBetiko = new Button {
                Text = "🗑️ BEHIN-BETIKO EZABATU",
                Location = new Point(800, 500),
                Size = new Size(350, 70),
                BackColor = Color.Salmon,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Visible = (erabiltzailea.Rola != "Irakaslea")
            };
            btBehinBetiko.Click += btBehinBetiko_Click;

            Label lblInfo = new Label {
                Text = "Oharra: 'EZABATUA' egoeran daudenak ezin dira berreskuratu (datu teknikoak borratu dira).",
                Location = new Point(50, 600),
                Size = new Size(1100, 40),
                ForeColor = Color.DarkRed,
                Font = new Font("Segoe UI", 11, FontStyle.Italic)
            };

            this.Controls.Add(dgBajaMantentze);
            this.Controls.Add(btBerreskuratu);
            this.Controls.Add(btMantenimendura);
            this.Controls.Add(btBehinBetiko);
            this.Controls.Add(lblInfo);
        }

        /// <summary>
        /// Gailuen taula datu-baseko informazioarekin eguneratzen du (hautsiak edo mantenimenduan daudenak).
        /// </summary>
        private void EguneratuTaula()
        {
            if (dgBajaMantentze != null)
                dgBajaMantentze.DataSource = kudeatzailea.LortuBajaEtaMantentzeSistema();
            EguneratuBotoiak();
        }

        /// <summary>
        /// Hautatutako gailuaren egoeraren arabera ekintza botoiak gaitzen edo desgaitzen ditu.
        /// </summary>
        private void EguneratuBotoiak()
        {
            if (dgBajaMantentze?.SelectedRows.Count > 0)
            {
                string egoera = dgBajaMantentze.SelectedRows[0].Cells["Egoera"].Value?.ToString() ?? "";
                bool irakasleaDa = (erabiltzailea.Rola == "Irakaslea");

                btBerreskuratu!.Enabled = !irakasleaDa && (egoera == "Mantentze-lanetan" || egoera == "Hautsia");
                btMantenimendura!.Enabled = !irakasleaDa && (egoera == "Hautsia");
                btBehinBetiko!.Enabled = !irakasleaDa && (egoera == "Mantentze-lanetan" || egoera == "Hautsia");
            }
        }

        /// <summary>
        /// Berreskuratu botoia sakatzean gailua berriro aktiboa izatera pasatzen du.
        /// </summary>
        /// <param name="igorlea">Gertaera sortu duen objektua.</param>
        /// <param name="e">Gertaeraren datuak.</param>
        private void btBerreskuratu_Click(object? igorlea, EventArgs e)
        {
            if (dgBajaMantentze?.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgBajaMantentze.SelectedRows[0].Cells["Identifikatzailea"].Value);
                kudeatzailea.GailuaEgoeraAldatu(id, "Aktiboa", erabiltzailea.IdErabiltzailea);
                MessageBox.Show("Gailua berriro aktibatu da eta inbentario orokorrera itzuli da.");
                EguneratuTaula();
            }
        }

        /// <summary>
        /// Mantenimendura botoia sakatzean gailuaren egoera aldatzen du.
        /// </summary>
        /// <param name="igorlea">Gertaera sortu duen objektua.</param>
        /// <param name="e">Gertaeraren datuak.</param>
        private void btMantenimendura_Click(object? igorlea, EventArgs e)
        {
            if (dgBajaMantentze?.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgBajaMantentze.SelectedRows[0].Cells["Identifikatzailea"].Value);
                kudeatzailea.GailuaEgoeraAldatu(id, "Mantentze-lanetan", erabiltzailea.IdErabiltzailea);
                MessageBox.Show("Gailua mantenimenduan dago orain.");
                EguneratuTaula();
            }
        }

        /// <summary>
        /// Behin betiko ezabatu botoia sakatzean gailua sistematik guztiz ezabatzen du.
        /// </summary>
        /// <param name="igorlea">Gertaera sortu duen objektua.</param>
        /// <param name="e">Gertaeraren datuak.</param>
        private void btBehinBetiko_Click(object? igorlea, EventArgs e)
        {
            if (dgBajaMantentze?.SelectedRows.Count > 0)
            {
                var errenkada = dgBajaMantentze.SelectedRows[0];
                int id = Convert.ToInt32(errenkada.Cells["Identifikatzailea"].Value);
                string izena = errenkada.Cells["Marka"].Value?.ToString() ?? "Gailua";
                string koka = errenkada.Cells["Kokalekua"].Value?.ToString() ?? "";
                DateTime data = Convert.ToDateTime(errenkada.Cells["Erosketa Data"].Value);

                if (MessageBox.Show($"{izena} BEHIN-BETIKO ezabatu nahi duzu?", "ADI", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    Gailua g = new Ordenagailua(id, izena, koka, "Ezabatua", data, "", 0); 
                    kudeatzailea.GailuaEzabatu(g, erabiltzailea.IdErabiltzailea);
                    EguneratuTaula();
                }
            }
        }
    }
}