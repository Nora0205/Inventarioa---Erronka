using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using StockBaseApp.Kontrolagailuak;
using MySql.Data.MySqlClient;

namespace StockBaseApp
{
    public class IoTForm : Form
    {
        private Konexioa konexioa;
        private Label lblTemp, lblHumedad, lblStatus;

        public IoTForm()
        {
            this.konexioa = new Konexioa();
            InitializeComponent();
            ActualizarDatos();
        }

        private void InitializeComponent()
        {
            this.Text = "StockBase IoT - Biltegi Monitorizazioa";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(44, 62, 80);

            Label title = new Label { 
                Text = "IoT Sentsoreak (Biltegia)", 
                ForeColor = Color.White, 
                Location = new Point(20, 20), 
                Size = new Size(340, 40), 
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblTemp = new Label { Text = "-- ºC", ForeColor = Color.LightSkyBlue, Location = new Point(50, 80), Size = new Size(300, 50), Font = new Font("Arial", 24, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            lblHumedad = new Label { Text = "-- %", ForeColor = Color.LightGreen, Location = new Point(50, 140), Size = new Size(300, 50), Font = new Font("Arial", 24, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            
            lblStatus = new Label { Text = "Baldintzak: Egonkorrak", ForeColor = Color.White, Location = new Point(50, 210), Size = new Size(300, 30), Font = new Font("Arial", 10, FontStyle.Italic), TextAlign = ContentAlignment.MiddleCenter };

            Button btnRefresh = new Button { Text = "Eguneratu Datuak", Location = new Point(100, 260), Size = new Size(200, 40), BackColor = Color.White };
            btnRefresh.Click += (s, e) => ActualizarDatos();

            this.Controls.AddRange(new Control[] { title, lblTemp, lblHumedad, lblStatus, btnRefresh });
        }

        private void ActualizarDatos()
        {
            try
            {
                using var conn = konexioa.LortuKonexioa();
                conn.Open();
                string sql = "SELECT izena, balioa, unitatea FROM Sensorrak";
                using var cmd = new MySqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string izena = reader["izena"].ToString()!;
                    string balioa = reader["balioa"].ToString()!;
                    string unitatea = reader["unitatea"].ToString()!;

                    if (izena.Contains("Tenperatura")) lblTemp.Text = $"{balioa} {unitatea}";
                    if (izena.Contains("Hezetasuna")) lblHumedad.Text = $"{balioa} {unitatea}";
                }
            }
            catch { lblStatus.Text = "Errorea datuak irakurtzean."; }
        }
    }
}