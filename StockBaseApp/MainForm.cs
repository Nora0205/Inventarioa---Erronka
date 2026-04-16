using System;
using System.Drawing;
using System.Windows.Forms;
using StockBaseApp.Modeloak;

namespace StockBaseApp
{
    public class MainForm : Form
    {
        private Erabiltzailea erabiltzailea;

        public MainForm(Erabiltzailea user)
        {
            this.erabiltzailea = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "StockBaseApp - Menú Nagusia";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label lblWelcome = new Label { 
                Text = $"Kaixo, {erabiltzailea.Izena}\n({erabiltzailea.Rola})", 
                Location = new Point(20, 20), 
                Size = new Size(440, 60), 
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            Button btnAddPC = new Button { Text = "Ordenagailua Gehitu", Location = new Point(100, 100), Size = new Size(300, 60), BackColor = Color.LightBlue };
            Button btnAddPrint = new Button { Text = "Inprimagailua Gehitu", Location = new Point(100, 180), Size = new Size(300, 60), BackColor = Color.LightGreen };
            Button btnView = new Button { Text = "Inbentarioa Ikusi", Location = new Point(100, 260), Size = new Size(300, 60), BackColor = Color.LightYellow };
            Button btnLogout = new Button { Text = "Saioa Itxi", Location = new Point(100, 450), Size = new Size(300, 40), BackColor = Color.LightCoral };

            btnAddPC.Click += (s, e) => new AddComputerForm(erabiltzailea).ShowDialog();
            btnAddPrint.Click += (s, e) => new AddPrinterForm(erabiltzailea).ShowDialog();
            btnView.Click += (s, e) => new ViewDevicesForm(erabiltzailea).ShowDialog();
            btnLogout.Click += (s, e) => this.Close();

            // Segurtasuna rolen arabera
            if (erabiltzailea.Rola == "Irakaslea")
            {
                btnAddPC.Enabled = false;
                btnAddPrint.Enabled = false;
            }

            this.Controls.Add(lblWelcome);
            this.Controls.Add(btnAddPC);
            this.Controls.Add(btnAddPrint);
            this.Controls.Add(btnView);
            this.Controls.Add(btnLogout);
        }
    }
}