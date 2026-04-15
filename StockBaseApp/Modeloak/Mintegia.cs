using System.Collections.Generic;

namespace StockBaseApp.Modeloak
{
    public class Mintegia
    {
        public int IdMintegia { get; set; }
        public string Izena { get; set; } = "";
        
        // Relación 1 --- * con Gailua (como en el diagrama)
        public List<Gailua> Gailuak { get; set; } = new List<Gailua>();

        public Mintegia(int id, string izena)
        {
            IdMintegia = id;
            Izena = izena;
        }
    }
}