using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public class MainForm : Form
    {
        private Erabiltzailea erabiltzailea;
        private Color primaryColor = Color.FromArgb(45, 52, 54);
        private Color backgroundColor = Color.FromArgb(241, 242, 246);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public MainForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent(); 
            InitCustomUI();        
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "MainForm";
            this.Text = "StockBaseApp";
            this.ResumeLayout(false);
        }

        private void InitCustomUI()
        {
            this.Size = new Size(500, 720); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 10);

            Panel headerPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = primaryColor
            };
            headerPanel.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); };

            Label lblWelcome = new Label { 
                Text = $"Kaixo, {erabiltzailea.Izena}\n{erabiltzailea.Rola}", 
                Location = new Point(50, 45),
                Size = new Size(400, 60),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 13, FontStyle.Bold)
            };

            Button btnExitApp = new Button {
                Text = "✕", Size = new Size(40, 40), Location = new Point(460, 0),
                FlatStyle = FlatStyle.Flat, ForeColor = Color.White, Cursor = Cursors.Hand
            };
            btnExitApp.FlatAppearance.BorderSize = 0;
            btnExitApp.MouseEnter += (s, e) => btnExitApp.BackColor = Color.Red;
            btnExitApp.MouseLeave += (s, e) => btnExitApp.BackColor = primaryColor;
            btnExitApp.Click += (s, e) => { 
                this.DialogResult = DialogResult.Abort; 
                this.Close(); 
            };

            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnExitApp);
            btnExitApp.BringToFront();

            Button btnAddPC = CreateStyledButton("🖥️  Ordenagailua Gehitu", 130, Color.FromArgb(0, 184, 148));
            Button btnAddPrint = CreateStyledButton("🖨️  Inprimagailua Gehitu", 205, Color.FromArgb(0, 206, 201));
            Button btnView = CreateStyledButton("📋  Inbentarioa Ikusi", 280, Color.FromArgb(253, 203, 110));
            Button btnManageUsers = CreateStyledButton("👥  Erabiltzaileak Kudeatu", 355, Color.FromArgb(108, 92, 231));
            
            Button btnSwitchUser = CreateStyledButton("🔄  Aldatu Erabiltzailea", 450, Color.FromArgb(9, 132, 227), true);
            Button btnLogout = CreateStyledButton("🚪  Saioa Itxi", 510, Color.FromArgb(214, 48, 49), true);

            try
            {
                string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logoa2.png");
                PictureBox pbLogo = new PictureBox {
                    Image = Image.FromFile(logoPath),
                    Size = new Size(150, 150),
                    Location = new Point(200, 590),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };
                this.Controls.Add(pbLogo);
            }
            catch { }

            btnAddPC.Click += (s, e) => new AddComputerForm(erabiltzailea).ShowDialog();
            btnAddPrint.Click += (s, e) => new AddPrinterForm(erabiltzailea).ShowDialog();
            btnView.Click += (s, e) => new ViewDevicesForm(erabiltzailea).ShowDialog();
            btnManageUsers.Click += (s, e) => new ManageUsersForm(erabiltzailea).ShowDialog();
            
            btnSwitchUser.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };
            btnLogout.Click += (s, e) => { this.DialogResult = DialogResult.Abort; this.Close(); };

            if (erabiltzailea.Rola == "Irakaslea")
            {
                btnAddPC.Enabled = false; btnAddPrint.Enabled = false;
                btnAddPC.BackColor = Color.FromArgb(180, 180, 180);
                btnAddPrint.BackColor = Color.FromArgb(180, 180, 180);
            }

            this.Controls.Add(headerPanel);
            this.Controls.Add(btnAddPC);
            this.Controls.Add(btnAddPrint);
            this.Controls.Add(btnView);
            this.Controls.Add(btnManageUsers);
            this.Controls.Add(btnSwitchUser);
            this.Controls.Add(btnLogout);
        }

        private Button CreateStyledButton(string text, int top, Color baseColor, bool isSmall = false)
        {
            Button btn = new Button {
                Text = text,
                Location = new Point(75, top),
                Size = new Size(350, isSmall ? 45 : 60),
                FlatStyle = FlatStyle.Flat,
                BackColor = baseColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", isSmall ? 10 : 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(baseColor);
            btn.MouseLeave += (s, e) => btn.BackColor = baseColor;
            return btn;
        }
    }
}