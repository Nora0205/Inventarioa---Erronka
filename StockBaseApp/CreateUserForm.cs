using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using StockBaseApp.Kontrolagailuak;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public class CreateUserForm : Form
    {
        private InbentarioSistema kudeatzailea;
        private Erabiltzailea creator;
        private TextBox? txtIzena, txtEmail, txtPass;
        private ComboBox? cmbRola, cmbMintegia;
        private Color primaryColor = Color.FromArgb(45, 52, 54);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public CreateUserForm(Erabiltzailea creator)
        {
            this.creator = creator;
            this.FormBorderStyle = FormBorderStyle.None;
            kudeatzailea = new InbentarioSistema();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(450, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 242, 246);
            this.Font = new Font("Segoe UI", 10);

            Panel headerPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = primaryColor
            };
            headerPanel.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); };

            Label lblTitle = new Label {
                Text = "➕ ERABILTZAILE BERRIA",
                Size = new Size(350, 60),
                Location = new Point(50, 0),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            headerPanel.Controls.Add(lblTitle);

            Button btnCloseX = new Button {
                Text = "✕", Size = new Size(40, 40), Location = new Point(400, 10),
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnCloseX.FlatAppearance.BorderSize = 0;
            btnCloseX.Click += (s, e) => this.Close();
            headerPanel.Controls.Add(btnCloseX);
            btnCloseX.BringToFront();

            int startY = 80;
            Label lblIzena = CreateLabel("Izena:", startY);
            txtIzena = CreateTextBox(startY + 25);

            Label lblEmail = CreateLabel("Email:", startY + 80);
            txtEmail = CreateTextBox(startY + 105);

            Label lblPass = CreateLabel("Pasahitza:", startY + 160);
            txtPass = CreateTextBox(startY + 185, true);

            Label lblRola = CreateLabel("Rola:", startY + 240);
            cmbRola = CreateComboBox(startY + 265);
            
            Label lblMintegia = CreateLabel("Mintegia:", startY + 320);
            cmbMintegia = CreateComboBox(startY + 345);
            CargarMintegiak();

            if (cmbRola != null && cmbMintegia != null) {
                if (creator.Rola == "Irakaslea") {
                    cmbRola.Items.Add("Irakaslea"); cmbRola.SelectedIndex = 0; cmbRola.Enabled = false;
                    SetMintegia(creator.MintegiJabea?.IdMintegia ?? 1); cmbMintegia.Enabled = false;
                } else if (creator.Rola == "Mintegi burua") {
                    cmbRola.Items.AddRange(new string[] { "Irakaslea", "Mintegi burua" }); cmbRola.SelectedIndex = 0;
                    SetMintegia(creator.MintegiJabea?.IdMintegia ?? 1); cmbMintegia.Enabled = false;
                } else {
                    cmbRola.Items.AddRange(new string[] { "Irakaslea", "Mintegi burua", "IKT arduraduna" }); cmbRola.SelectedIndex = 0;
                }
            }

            Button btnGorde = new Button {
                Text = "💾 ERABILTZAILEA GORDE",
                Location = new Point(50, 480),
                Size = new Size(350, 50),
                BackColor = Color.FromArgb(0, 184, 148),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGorde.FlatAppearance.BorderSize = 0;
            btnGorde.Click += btnGorde_Click;

            this.Controls.AddRange(new Control[] { headerPanel, lblIzena, txtIzena, lblEmail, txtEmail, lblPass, txtPass, lblRola, cmbRola, lblMintegia, cmbMintegia, btnGorde });
        }

        private Label CreateLabel(string text, int top) => new Label { Text = text, Location = new Point(50, top), AutoSize = true, ForeColor = Color.FromArgb(99, 110, 114) };
        private TextBox CreateTextBox(int top, bool isPass = false) => new TextBox { Location = new Point(50, top), Size = new Size(350, 30), Font = new Font("Segoe UI", 11), PasswordChar = isPass ? '*' : '\0' };
        private ComboBox CreateComboBox(int top) => new ComboBox { Location = new Point(50, top), Size = new Size(350, 30), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };

        private void CargarMintegiak()
        {
            if (cmbMintegia == null) return;
            var lista = kudeatzailea.LortuMintegiak();
            cmbMintegia.DisplayMember = "Value"; cmbMintegia.ValueMember = "Key";
            foreach (var item in lista) cmbMintegia.Items.Add(item);
            if (cmbMintegia.Items.Count > 0) cmbMintegia.SelectedIndex = 0;
        }

        private void SetMintegia(int id)
        {
            if (cmbMintegia == null) return;
            for (int i = 0; i < cmbMintegia.Items.Count; i++) {
                if (cmbMintegia.Items[i] is KeyValuePair<int, string> item && item.Key == id) { cmbMintegia.SelectedIndex = i; break; }
            }
        }

        private void btnGorde_Click(object? sender, EventArgs e)
        {
            try {
                if (txtIzena == null || txtEmail == null || txtPass == null || cmbRola == null || cmbMintegia == null) return;
                
                if (string.IsNullOrWhiteSpace(txtIzena.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Text)) {
                    MessageBox.Show("Mesedez, bete eremu guztiak."); return;
                }
                
                string rola = cmbRola.SelectedItem?.ToString() ?? "Irakaslea";
                int idMintegia = (cmbMintegia.SelectedItem is KeyValuePair<int, string> selectedItem) ? selectedItem.Key : 1;

                kudeatzailea.ErabiltzaileaGehitu(txtIzena.Text, txtEmail.Text, txtPass.Text, rola, idMintegia);
                MessageBox.Show("Erabiltzailea ondo gorde da!"); this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Errorea: " + ex.Message); }
        }
    }
}