using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    /// <summary>
    /// Sisteman erregistratuta dauden erabiltzaile guztien zerrenda erakusten duen formularioa.
    /// Administratzaileei erabiltzaileen datuak (izena, emaila, rola, mintegia) modu grafikoan ikusteko aukera ematen die.
    /// </summary>
    public class ViewUsersForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private DataGridView? dgErabiltzaileak;

        /// <summary>
        /// ViewUsersForm-en instantzia berri bat sortzen du eta datuak kargatzen ditu.
        /// </summary>
        public ViewUsersForm()
        {
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
            EguneratuTaula();
        }

        /// <summary>
        /// Formularioaren interfaze grafikoa eta erabiltzaileen taula (Grid) hasieratzen ditu.
        /// </summary>
        private void InterfazeaHasieratu()
        {
            this.Text = "Erabiltzaile Zerrenda";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgErabiltzaileak = new DataGridView { 
                Dock = DockStyle.Fill, 
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgErabiltzaileak);
        }

        /// <summary>
        /// Erabiltzaileen taula eguneratzen du datu-basetik lortutako azken informazioarekin.
        /// </summary>
        private void EguneratuTaula()
        {
            if (dgErabiltzaileak != null)
                dgErabiltzaileak.DataSource = kudeatzailea.ErabiltzaileGuztiakLortu();
        }
    }
}