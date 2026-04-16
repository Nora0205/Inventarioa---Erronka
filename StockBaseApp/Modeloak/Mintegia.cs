using System.Collections.Generic;

namespace StockBaseApp.Modeloak
{
    public class Mintegia(int id, string izena)
    {
        public int IdMintegia { get; set; } = id;
        public string Izena { get; set; } = izena;

        // Relación 1 --- * con Gailua (como en el diagrama)
        public List<Gailua> Gailuak { get; set; } = [];
    }
}