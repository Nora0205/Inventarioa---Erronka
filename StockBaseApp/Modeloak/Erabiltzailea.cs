using System.Collections.Generic;

namespace StockBaseApp.Modeloak
{
    public class Erabiltzailea(int id, string izena, string email, string rola, string pasahitza = "")
    {
        public int IdErabiltzailea { get; set; } = id;
        public string Izena { get; set; } = izena;
        public string Email { get; set; } = email;
        public string Pasahitza { get; set; } = pasahitza;
        public string Rola { get; set; } = rola;
        public Mintegia? MintegiJabea { get; set; }
    }
}