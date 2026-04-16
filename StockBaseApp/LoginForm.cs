using System;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public partial class LoginForm : Form
    {
        private readonly InbentarioSistema kudeatzailea;
        public Erabiltzailea? LoggedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            kudeatzailea = new InbentarioSistema();
            LoggedUser = null;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string izena = txtEmail.Text; // Mantenemos el nombre del control txtEmail para no romper el Designer
            string pass = txtPass.Text;

            if (string.IsNullOrEmpty(izena) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Mesedez, bete eremu guztiak.");
                return;
            }

            var usuario = kudeatzailea.SaioaHasi(izena, pass);
            if (usuario != null)
            {
                LoggedUser = usuario;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Erabiltzailea edo pasahitza okerra.");
            }
        }
    }
}