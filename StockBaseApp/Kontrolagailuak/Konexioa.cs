using MySql.Data.MySqlClient;

namespace StockBaseApp.Kontrolagailuak
{
    public class Konexioa
    {
        private string servidor = "DESKTOP-7C5QQLJ"; // Valor por defecto
        private string bd = "Inventarioa";
        private string erabiltzailea = "root";
        private string pasahitza = "root";

        public Konexioa()
        {
            // Intentar leer la IP desde un archivo local para que sea fácil de cambiar
            string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
            if (System.IO.File.Exists(configPath))
            {
                string contenido = System.IO.File.ReadAllText(configPath).Trim();
                if (!string.IsNullOrEmpty(contenido)) servidor = contenido;
            }
        }

        public MySqlConnection LortuKonexioa()
        {
            // Añadimos AllowPublicKeyRetrieval=True para corregir el error de la captura de pantalla
            string konexioKatea = $"Server={servidor};Database={bd};Uid={erabiltzailea};Pwd={pasahitza};Port=3306;SslMode=Disabled;AllowUserVariables=True;AllowPublicKeyRetrieval=True;";
            return new MySqlConnection(konexioKatea);
        }
    }}