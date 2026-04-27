using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    /// <summary>
    /// Erabiltzaileen kudeaketa orokorra egiteko menuko formularioa.
    /// Hemendik erabiltzaileak ikusi, sortu edo ezabatzeko formularioetara sartzen da.
    /// </summary>
    public class ManageUsersForm : Form
    {
        private Erabiltzailea erabiltzailea;
        private Color primaryColor = Color.FromArgb(45, 52, 54);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// ManageUsersForm-en instantzia berri bat sortzen du.
        /// </summary>
        /// <param name="user">Unean saioa hasita duen erabiltzailea (baimenak egiaztatzeko).</param>
        public ManageUsersForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            this.FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
        }

        /// <summary>
        /// Formularioaren interfaze grafikoa eta botoien ekintzak inizializatzen ditu.
        /// </summary>
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
            btnDelete.Click += (s, e) => new DeleteUserForm(erabiltzailea).ShowDialog();
            btnClose.Click += (s, e) => this.Close();

            if (erabiltzailea.Rola == "Irakaslea") btnDelete.Enabled = false;

            this.Controls.AddRange(new Control[] { headerPanel, btnView, btnCreate, btnDelete, btnClose });
        }

        /// <summary>
        /// Estilo uniformea duten botoiak sortzeko laguntza-metodoa.
        /// </summary>
        /// <param name="text">Botoiaren testua.</param>
        /// <param name="top">Goiko margenaren posizioa.</param>
        /// <param name="baseColor">Botoiaren atzeko kolorea.</param>
        /// <param name="isSmall">Tamaina txikia (40px) edo handia (60px) den.</param>
        /// <returns>Konfiguratutako <see cref="Button"/> objektua.</returns>
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