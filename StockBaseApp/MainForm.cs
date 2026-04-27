using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Necesario para bordes redondeados y degradados
using System.Windows.Forms;
using System.Runtime.InteropServices;
using StockBaseApp.Modeloak;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    /// <summary>
    /// Aplikazioaren inprimaki nagusia. Hemendik gainerako aukera guztietara sar daiteke.
    /// </summary>
    public class MainForm : Form
    {
        private Erabiltzailea erabiltzailea;
        private Color primaryColor = Color.FromArgb(20, 30, 48); // Azul profundo
        private Color buttonBaseColor = Color.FromArgb(44, 62, 80); // Gris azulado metálico
        private Color backgroundColor = Color.FromArgb(15, 15, 15); // Fondo casi negro para que resalte el brillo
        private Color highlightColor = Color.FromArgb(0, 184, 148); // Verde neón para el botón principal

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// MainForm klasearen instantzia berria sortzen du.
        /// </summary>
        /// <param name="user">Sisteman saioa hasi duen erabiltzailea.</param>
        public MainForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized; 
            InitializeComponent();
            InitCustomUI();
        }

        /// <summary>
        /// Inprimakiaren oinarrizko kontrolak hasieratzen ditu.
        /// </summary>
        private void InitializeComponent()        {
            this.SuspendLayout();
            this.Name = "MainForm";
            this.Text = "StockBaseApp";
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Inprimakiaren interfaze grafiko pertsonalizatua eta modernoa hasieratzen du.
        /// </summary>
        private void InitCustomUI()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 10);

            // 1. Goiburua Gradientearekin
            Panel headerPanel = new Panel { Dock = DockStyle.Top, Height = 120 };
            headerPanel.Paint += (s, e) => {
                LinearGradientBrush lgb = new LinearGradientBrush(headerPanel.ClientRectangle, primaryColor, Color.Black, 90f);
                e.Graphics.FillRectangle(lgb, headerPanel.ClientRectangle);
            };

            Label lblWelcome = new Label { 
                Text = $"STOCKBASE SYSTEM | {erabiltzailea.Izena.ToUpper()}\n{erabiltzailea.Rola} • {erabiltzailea.MintegiJabea?.Izena ?? "ADMIN"}", 
                Dock = DockStyle.Fill, ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Light", 18), BackColor = Color.Transparent
            };
            
            Button btnExitApp = new Button {
                Text = "✕", Size = new Size(80, 120), Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat, ForeColor = Color.Gray, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 16), BackColor = Color.Transparent
            };
            btnExitApp.FlatAppearance.BorderSize = 0;
            btnExitApp.MouseEnter += (s, e) => btnExitApp.ForeColor = Color.Red;
            btnExitApp.MouseLeave += (s, e) => btnExitApp.ForeColor = Color.Gray;
            btnExitApp.Click += (s, e) => { this.DialogResult = DialogResult.Abort; this.Close(); };
            
            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnExitApp);

            // 2. Kontenedore Nagusia (Zabalagoa eta tarte gehiagorekin)
            Panel mainLayout = new Panel { Size = new Size(1100, 750), BackColor = Color.Transparent };
            this.Resize += (s, e) => mainLayout.Location = new Point((this.Width - mainLayout.Width) / 2, (this.Height - headerPanel.Height - mainLayout.Height) / 2 + headerPanel.Height);
            this.Load += (s, e) => mainLayout.Location = new Point((this.Width - mainLayout.Width) / 2, (this.Height - headerPanel.Height - mainLayout.Height) / 2 + headerPanel.Height);

            // INBENTARIOA 
            Button btnView = CreateStyledButton("📋 INBENTARIOAREN KUDEAKETA OSOA", highlightColor, new Size(1060, 160), 22, 20);
            btnView.Location = new Point(20, 20);
            btnView.Click += (s, e) => new ViewDevicesForm(erabiltzailea).ShowDialog();

            // GAILUEN KUDEAKETA
            Label lblGailuak = new Label { Text = "GAILUEN ERREGISTROA", Location = new Point(30, 210), Size = new Size(400, 30), Font = new Font("Segoe UI Black", 10), ForeColor = Color.DarkGray };
            Button btnAddPC = CreateStyledButton("🖥️ Ordenagailu Berria", buttonBaseColor, new Size(515, 130), 15, 15);
            btnAddPC.Location = new Point(20, 240);
            btnAddPC.Click += (s, e) => new AddComputerForm(erabiltzailea).ShowDialog();

            Button btnAddPrint = CreateStyledButton("🖨️ Inprimagailu Berria", buttonBaseColor, new Size(515, 130), 15, 15);
            btnAddPrint.Location = new Point(565, 240);
            btnAddPrint.Click += (s, e) => new AddPrinterForm(erabiltzailea).ShowDialog();

            // ADMINISTRAZIOA
            Label lblAdmin = new Label { Text = "SISTEMAREN ADMINISTRAZIOA", Location = new Point(30, 400), Size = new Size(400, 30), Font = new Font("Segoe UI Black", 10), ForeColor = Color.DarkGray };
            Button btnManageUsers = CreateStyledButton("👥 Erabiltzaileak", Color.FromArgb(60, 60, 60), new Size(515, 90), 12, 12);
            btnManageUsers.Location = new Point(20, 430);
            btnManageUsers.Click += (s, e) => new ManageUsersForm(erabiltzailea).ShowDialog();

            Button btnManageMintegiak = CreateStyledButton("🏫 Mintegiak", Color.FromArgb(60, 60, 60), new Size(515, 90), 12, 12);
            btnManageMintegiak.Location = new Point(565, 430);
            btnManageMintegiak.Click += (s, e) => new ManageMintegiakForm(erabiltzailea).ShowDialog();

            // BOTOI TXIKIAK 
            Button btnSwitchUser = CreateStyledButton("🔄 Aldatu Saioa", Color.FromArgb(40, 40, 40), new Size(515, 60), 11, 10);
            btnSwitchUser.Location = new Point(20, 560);
            btnSwitchUser.Click += (s, e) => { this.DialogResult = DialogResult.Retry; this.Close(); };

            Button btnLogout = CreateStyledButton("🚪 Itxi Sistema", Color.FromArgb(100, 20, 20), new Size(515, 60), 11, 10);
            btnLogout.Location = new Point(565, 560);
            btnLogout.Click += (s, e) => { this.DialogResult = DialogResult.Abort; this.Close(); };

            if (erabiltzailea.Rola == "Irakaslea")
            {
                btnAddPC.Enabled = btnAddPrint.Enabled = btnManageUsers.Enabled = btnManageMintegiak.Enabled = false;
                btnAddPC.BackColor = btnAddPrint.BackColor = btnManageUsers.BackColor = btnManageMintegiak.BackColor = Color.FromArgb(30, 30, 30);
            }

            mainLayout.Controls.AddRange(new Control[] { btnView, lblGailuak, btnAddPC, btnAddPrint, lblAdmin, btnManageUsers, btnManageMintegiak, btnSwitchUser, btnLogout });

            this.Controls.Add(mainLayout);
            this.Controls.Add(headerPanel);
        }

        /// <summary>
        /// Estilo berezia eta itxura modernoa duen botoi bat sortzen du.
        /// </summary>
        /// <param name="text">Botoian bistaratuko den testua.</param>
        /// <param name="baseColor">Botoiaren oinarrizko kolorea.</param>
        /// <param name="size">Botoiaren tamaina.</param>
        /// <param name="fontSize">Testuaren letra-tamaina.</param>
        /// <param name="radius">Botoiaren ertzen biribiltze-erradioa.</param>
        /// <returns>Sortutako botoi pertsonalizatua.</returns>
        private Button CreateStyledButton(string text, Color baseColor, Size size, int fontSize, int radius)
        {
            Button btn = new Button {
                Text = text, Size = size, FlatStyle = FlatStyle.Flat, BackColor = baseColor,
                ForeColor = Color.White, Font = new Font("Segoe UI Semibold", fontSize),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
                Rectangle glossRect = new Rectangle(0, 0, btn.Width, btn.Height / 2);

                // 1. Bide borobildua sortu
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);

                // 2. Fondoa gradiente leunarekin (Sakonera)
                using (LinearGradientBrush lgb = new LinearGradientBrush(rect, ControlPaint.Light(baseColor), ControlPaint.Dark(baseColor), 90f))
                {
                    e.Graphics.FillPath(lgb, path);
                }

                // 3. Goiko distira (Glossy)
                using (LinearGradientBrush gloss = new LinearGradientBrush(glossRect, Color.FromArgb(80, Color.White), Color.Transparent, 90f))
                {
                    e.Graphics.FillRectangle(gloss, glossRect);
                }

                // 4. Beheko sakonera bordea
                using (Pen p = new Pen(Color.FromArgb(100, Color.Black), 2))
                {
                    e.Graphics.DrawArc(p, 1, btn.Height - radius - 1, radius, radius, 90, 90);
                    e.Graphics.DrawLine(p, radius, btn.Height - 1, btn.Width - radius, btn.Height - 1);
                    e.Graphics.DrawArc(p, btn.Width - radius - 1, btn.Height - radius - 1, radius, radius, 0, 90);
                }

                // 5. Testua eskuz marraztu erdian eta garbi egon dadin
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.LightLight(baseColor);
            btn.MouseLeave += (s, e) => btn.BackColor = baseColor;
            return btn;
        }
    }
}