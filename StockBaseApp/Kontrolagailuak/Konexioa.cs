using MySql.Data.MySqlClient;

namespace StockBaseApp.Kontrolagailuak
{
    public class Konexioa
    {
        private string konexioKatea = "Server=localhost;Database=Inventarioa;Uid=root;Pwd=root;";
        
        public MySqlConnection LortuKonexioa()
        {
            return new MySqlConnection(konexioKatea);
        }
    }
}