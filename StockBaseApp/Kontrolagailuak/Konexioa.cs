using MySql.Data.MySqlClient;

namespace StockBaseApp.Kontrolagailuak
{
    public class Konexioa
    {
        private string zerbitzaria = "DESKTOP-7C5QQLJ"; 
        private string datuBasea = "Inventarioa";
        private string erabiltzailea = "root";
        private string pasahitza = "root";

        public Konexioa()
        {
            string konfigurazioBidea = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            if (System.IO.File.Exists(konfigurazioBidea))
            {
                string edukia = System.IO.File.ReadAllText(konfigurazioBidea).Trim();
                if (!string.IsNullOrEmpty(edukia)) 
                {
                    zerbitzaria = edukia;
                }
            }
        }

        public MySqlConnection LortuKonexioa()
        {
            string konexioKatea = $"Server={zerbitzaria};Database={datuBasea};Uid={erabiltzailea};Pwd={pasahitza};Port=3306;SslMode=Disabled;AllowUserVariables=True;AllowPublicKeyRetrieval=True;Connection Timeout=5;";
            return new MySqlConnection(konexioKatea);
        }
    }
}