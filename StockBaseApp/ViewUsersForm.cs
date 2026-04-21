using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public class ViewUsersForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private DataGridView? dgErabiltzaileak;

        public ViewUsersForm()
        {
            this.kudeatzailea = new InbentarioSistema();
            InterfazeaHasieratu();
            EguneratuTaula();
        }

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

        private void EguneratuTaula()
        {
            if (dgErabiltzaileak != null)
                dgErabiltzaileak.DataSource = kudeatzailea.ErabiltzaileGuztiakLortu();
        }
    }
}