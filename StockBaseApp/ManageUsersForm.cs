using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public class ManageUsersForm : Form
    {
        private Erabiltzailea erabiltzailea;
        private Color primaryColor = Color.FromArgb(45, 52, 54);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public ManageUsersForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(241, 242, 246);
            this.Font = new Font("Segoe UI", 10);

            Panel headerPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = primaryColor
            };
            headerPanel.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); };

            Label lblTitle = new Label {
                Text = "ERABILTZAILEEN KUDEAKETA",
                Size = new Size(400, 80),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            headerPanel.Controls.Add(lblTitle);

            Button btnView = CreateStyledButton("👁️ Erabiltzaileak Ikusi", 100, Color.FromArgb(253, 203, 110));
            Button btnCreate = CreateStyledButton("➕ Erabiltzailea Sortu", 180, Color.FromArgb(9, 132, 227));
            Button btnDelete = CreateStyledButton("🗑️ Erabiltzailea Ezabatu", 260, Color.FromArgb(214, 48, 49));
            Button btnClose = CreateStyledButton("⬅️ Itzuli", 340, Color.FromArgb(99, 110, 114), true);

            btnView.Click += (s, e) => new ViewUsersForm().ShowDialog();
            btnCreate.Click += (s, e) => new CreateUserForm(erabiltzailea).ShowDialog();
            btnDelete.Click += (s, e) => new DeleteUserForm().ShowDialog();
            btnClose.Click += (s, e) => this.Close();

            if (erabiltzailea.Rola == "Irakaslea") btnDelete.Enabled = false;

            this.Controls.AddRange(new Control[] { headerPanel, btnView, btnCreate, btnDelete, btnClose });
        }

        private Button CreateStyledButton(string text, int top, Color baseColor, bool isSmall = false)
        {
            Button btn = new Button {
                Text = text,
                Location = new Point(50, top),
                Size = new Size(300, isSmall ? 40 : 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = baseColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(baseColor);
            btn.MouseLeave += (s, e) => btn.BackColor = baseColor;
            return btn;
        }
    }
}