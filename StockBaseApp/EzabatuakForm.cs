using System;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    public partial class EzabatuakForm : Form
    {
        private InbentarioSistema kudeatzailea;

        public EzabatuakForm()
        {
            InitializeComponent();
            kudeatzailea = new InbentarioSistema();
            CargarDatos();
        }

        private void CargarDatos()
        {
            dgvEzabatuak.DataSource = kudeatzailea.LortuEzabatutakoGailuak();
        }
    }
}