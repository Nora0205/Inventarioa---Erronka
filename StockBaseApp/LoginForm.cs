using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    /// <summary>
    /// Sisteman saioa hasteko inprimakia.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly InbentarioSistema kudeatzailea;
        /// <summary>
        /// Saioa hasi duen erabiltzailea. Null da saioa hasi ez bada.
        /// </summary>
        public Erabiltzailea? LoggedUser { get; private set; }
        private Color primaryColor = Color.FromArgb(45, 52, 54);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// LoginForm klasearen instantzia berria sortzen du.
        /// </summary>
        public LoginForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            kudeatzailea = new InbentarioSistema();
            LoggedUser = null;
            InitModernUI();
        }

        /// <summary>
        /// Inprimakiaren interfaze grafiko modernoa hasieratzen du.
        /// </summary>
        private void InitModernUI()
        {
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 242, 246);
            this.Font = new Font("Segoe UI", 10);

            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = primaryColor
            };
            headerPanel.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); };

            Label lblLogo = new Label
            {
                Text = "STOCKBASE",
                Size = new Size(300, 120),
                Location = new Point(50, 0),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 24, FontStyle.Bold)
            };
            headerPanel.Controls.Add(lblLogo);

            Button btnClose = new Button
            {
                Text = "✕",
                Size = new Size(40, 40),
                Location = new Point(360, 0),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            headerPanel.Controls.Add(btnClose);
            btnClose.BringToFront();

            Label lblIzena = new Label { Text = "Erabiltzaile Izena", Location = new Point(50, 160), AutoSize = true, ForeColor = Color.FromArgb(99, 110, 114) };
            TextBox txtIzena = new TextBox { Name = "txtEmail", Location = new Point(50, 185), Size = new Size(300, 30), Font = new Font("Segoe UI", 12) };

            Label lblPass = new Label { Text = "Pasahitza", Location = new Point(50, 240), AutoSize = true, ForeColor = Color.FromArgb(99, 110, 114) };
            TextBox txtPasahitza = new TextBox { Name = "txtPass", Location = new Point(50, 265), Size = new Size(300, 30), Font = new Font("Segoe UI", 12), PasswordChar = '*' };

            Button btnLogin = new Button
            {
                Text = "SAIOA HASI",
                Location = new Point(50, 350),
                Size = new Size(300, 50),
                BackColor = Color.FromArgb(9, 132, 227),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) =>
            {
                var usuario = kudeatzailea.SaioaHasi(txtIzena.Text, txtPasahitza.Text);
                if (usuario != null)
                {
                    LoggedUser = usuario;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Datuak ez dira zuzenak.");
                }
            };

            this.Controls.AddRange(new Control[] { headerPanel, lblIzena, txtIzena, lblPass, txtPasahitza, btnLogin });
        }
    }
}