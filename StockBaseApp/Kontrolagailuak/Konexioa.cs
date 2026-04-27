using MySql.Data.MySqlClient;

namespace StockBaseApp.Kontrolagailuak
{
    /// <summary>
    /// MySQL datu-basearekiko konexioa kudeatzen duen klasea.
    /// Zerbitzariaren helbidea 'config.txt' fitxategitik irakurtzen du konfigurazioa dinamikoa izan dadin.
    /// </summary>
    public class Konexioa
    {
        private string zerbitzaria = "DESKTOP-7C5QQLJ"; 
        private string datuBasea = "Inventarioa";
        private string erabiltzailea = "root";
        private string pasahitza = "root";

        /// <summary>
        /// Konexioa klasearen eraikitzailea.
        /// Exekuzio bidean 'config.txt' fitxategia badago, zerbitzariaren helbidea bertatik kargatzen du.
        /// </summary>
        public Konexioa()
        {
            // Konfigurazio fitxategiaren bidea lortu (aplikazioaren karpeta berean)
            string konfigurazioBidea = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            if (System.IO.File.Exists(konfigurazioBidea))
            {
                // Fitxategia badago, edukia irakurri eta zerbitzari gisa erabili
                string edukia = System.IO.File.ReadAllText(konfigurazioBidea).Trim();
                if (!string.IsNullOrEmpty(edukia)) 
                {
                    zerbitzaria = edukia;
                }
            }
        }

        /// <summary>
        /// MySQL-ra konektatzeko objektu berri bat sortu eta itzultzen du.
        /// </summary>
        /// <returns><see cref="MySqlConnection"/> objektu bat konfigurazio-katearekin.</returns>
        public MySqlConnection LortuKonexioa()
        {
            string konexioKatea = $"Server={zerbitzaria};Database={datuBasea};Uid={erabiltzailea};Pwd={pasahitza};Port=3306;SslMode=Disabled;AllowUserVariables=True;AllowPublicKeyRetrieval=True;Connection Timeout=5;";
            return new MySqlConnection(konexioKatea);
        }
    }
}