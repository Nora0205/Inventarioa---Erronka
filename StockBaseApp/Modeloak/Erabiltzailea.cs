namespace StockBaseApp.Modeloak
{
    public class Erabiltzailea
    {
        public int IdErabiltzailea { get; set; }
        public string Izena { get; set; } = "";
        public string Email { get; set; } = "";
        public string Rola { get; set; } = "";

        public Erabiltzailea(int id, string izena, string email, string rola)
        {
            IdErabiltzailea = id;
            Izena = izena;
            Email = email;
            Rola = rola;
        }
    }
}