using System;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public partial class LoginForm : Form
    {
        private InbentarioKudeatzailea kudeatzailea;
        public Erabiltzailea LoggedUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            kudeatzailea = new InbentarioKudeatzailea();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string pass = txtPass.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Mesedez, bete eremu guztiak.");
                return;
            }

            var usuario = kudeatzailea.SaioaHasi(email, pass);
            if (usuario != null)
            {
                LoggedUser = usuario;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Email edo pasahitza okerra.");
            }
        }
    }
}